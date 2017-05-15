using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using MySql.Data.MySqlClient;

namespace TaskQuest.App_Code
{
    public static class Cursor
    {

        private static MySqlConnection _connection;
        private static MySqlCommand _command;
        private static MySqlDataAdapter _adapter;

        private static string _type;
        private static Object _obj;
        private static int? _id;

        public static bool ExecuteQuery(Object obj, string type)
        {
            _type = type;
            _obj = obj;
            if (_type.Equals("Create") || _type.Equals("Update") || _type.Equals("Delete"))
            {
                try
                {
                    _connection = new MySqlConnection(ConfigurationManager.AppSettings["DefaultConnection"]);
                    _connection.Open();
                    _command = _connection.CreateCommand();
                    AddQuery();
                    AddParameters();
                    Execute();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            throw new Exception("Invalid Type");
        }

        public static List<T> ExecuteQuery<T>(Object obj, int? id = null) where T : new()
        {
            _type = "Read";
            _obj = obj;
            _id = id;
            try
            {
                _connection = new MySqlConnection(ConfigurationManager.AppSettings["DefaultConnection"]);
                _connection.Open();
                _command = _connection.CreateCommand();
                _adapter = new MySqlDataAdapter();
                AddQuery();
                AddParameters();
                return ExecuteSelect<T>();
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        private static void AddQuery()
        {
            var props = _obj.GetType().GetProperties();
            string query;
            if (_type.Equals("Insert"))
            {
                query = "INSERT INTO " + _obj.GetType().Name + "(" + Columns(props) +
                    ") VALUES(" + Columns(props, "?") + ")";
                _command.CommandText = query;
            }
            else if (_type.Equals("Select"))
            {
                query = "SELECT * FROM " + _obj.GetType().Name;
                if (_id != null)
                    query += " WHERE " + props[0].Name + " = " + _id;
                _command.CommandText = query;
            }
            else if (_type.Equals("Update"))
            {
                query = "UPDATE " + _obj.GetType().Name + " SET ";
                for (int x = 1; x < props.Length; x++)
                {
                    query += props[x].Name + " = ?" + props[x].Name;
                    if (x < props.Length - 1)
                        query += ", ";
                }
                query += " WHERE " + props[0].Name + " = " + props[0].GetValue(_obj);
                _command.CommandText = query;
            }
            else if (_type.Equals("Delete"))
            {
                query = "DELETE FROM " + _obj.GetType().Name +
                    " WHERE " + props[0].Name + " = " + props[0].GetValue(_obj);
                _command.CommandText = query;
            }
        }

        private static void AddParameters()
        {
            foreach (PropertyInfo prop in _obj.GetType().GetProperties())
                _command.Parameters.Add(new MySqlParameter(prop.Name, prop.GetValue(_obj)));
        }

        private static void Execute()
        {
            _command.ExecuteNonQuery();
            _connection.Close();
            _command.Dispose();
            _connection.Dispose();
        }

        private static List<T> ExecuteSelect<T>() where T : new()
        {
            var ds = new DataSet();
            _adapter.SelectCommand = _command;
            _adapter.Fill(ds);
            
            List<T> vetor = ds.Tables[0].ToList<T>();
            
            _connection.Close();
            _command.Dispose();
            _adapter.Dispose();
            _connection.Dispose();
            return vetor;
        }

        private static string Columns(PropertyInfo[] props, string plus="")
        {
            var query = "";
            for (int x = 1; x < props.Length; x++)
            {
                query += plus + props[x].Name;
                if (x < props.Length - 1)
                    query += ", ";
            }
            return query;
        }

        private static List<T> ToList<T>(this DataTable table) where T : new()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            List<T> result = new List<T>();

            foreach (var row in table.Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties);
                result.Add(item);
            }

            return result;
        }

        private static T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties) where T : new()
        {
            T item = new T();
            foreach (var property in properties)
            {
                property.SetValue(item, row[property.Name], null);
            }
            return item;
        }
    }
}