using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using MySql.Data.MySqlClient;

namespace TaskQuest.App_Code
{
    public static class Cursor
    {

        private static MySqlConnection _connection;
        private static MySqlCommand _command;

        private static Object _obj;

        public static bool Insert(Object obj)
        {
            _obj = obj;
            var props = _obj.GetType().GetProperties();

            var query = "INSERT INTO " + _obj.GetType().Name + "(" + Columns(props) +
                    ") VALUES(" + Columns(props, "?") + ")";

            return ExecuteQuery(query);
        }

        public static List<T> Select<T>(PropertyInfo atributo=null, Object value=null) where T : new()
        {
            _obj = new T();

            try
            {
                string myConnectionString = "server=127.0.0.1;uid=root;pwd=Kur0N3k0;database=task_quest;";
                _connection = new MySqlConnection
                {
                    ConnectionString = myConnectionString
                };
                _connection.Open();
                _command = _connection.CreateCommand();
                var adapter = new MySqlDataAdapter();

                var query = "SELECT * FROM " + _obj.GetType().Name;

                if (atributo != null && value != null)
                {
                    atributo.SetValue(_obj, value);
                    query += " WHERE " + atributo.Name + " = ?" + atributo.Name;
                }

                _command.CommandText = query;
                AddParameters();

                var ds = new DataSet();
                adapter.SelectCommand = _command;
                adapter.Fill(ds);

                List<T> vetor = ds.Tables[0].ToList<T>();

                _connection.Close();
                _command.Dispose();
                adapter.Dispose();
                _connection.Dispose();
                return vetor;

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        public static bool Update(Object obj)
        {
            _obj = obj;
            var props = _obj.GetType().GetProperties();

            var query = "UPDATE " + _obj.GetType().Name + " SET ";
            for (int x = 1; x < props.Length; x++)
            {
                query += props[x].Name + " = ?" + props[x].Name;
                if (x < props.Length - 1)
                    query += ", ";
            }
            query += " WHERE " + props[0].Name + " = " + props[0].GetValue(_obj);

            return ExecuteQuery(query);
        }

        public static bool Delete<T>(int id) where T : new()
        {
            _obj = new T();
            var props = _obj.GetType().GetProperties();

            if (id < 0)
                return false;

            var query = "DELETE FROM " + _obj.GetType().Name +
                    " WHERE " + props[0].Name + " = " + id;

            return ExecuteQuery(query);
        }

        private static bool ExecuteQuery(string query)
        {
            try
            {
                string myConnectionString = "server=127.0.0.1;uid=root;pwd=Kur0N3k0;database=task_quest;";
                _connection = new MySqlConnection
                {
                    ConnectionString = myConnectionString
                };
                _connection.Open();
                _command = _connection.CreateCommand();
                _command.CommandText = query;
                AddParameters();

                _command.ExecuteNonQuery();
                _connection.Close();
                _command.Dispose();
                _connection.Dispose();

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
        
        private static void AddParameters()
        {
            foreach (PropertyInfo prop in _obj.GetType().GetProperties())
                _command.Parameters.Add(new MySqlParameter(prop.Name, prop.GetValue(_obj)));
        }

        private static string Columns(PropertyInfo[] props, string plus = "")
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

            foreach (DataRow row in table.Rows)
            {
                T item = new T();
                foreach (var property in properties)
                {
                    property.SetValue(item, row[property.Name], null);
                }
                result.Add(item);
            }

            return result;
        }
    }
}
