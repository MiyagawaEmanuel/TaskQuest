using System;
using System.Configuration;
using System.Data;
using System.Reflection;
using MySql.Data.MySqlClient;

namespace TaskQuest.App_Code
{
    public static class Cursor{

        private static MySqlConnection Connection;
        private static MySqlCommand Command;
        private static MySqlDataAdapter Adapter;

        private static string Type;
        private static Object Obj;
        private static int Qtd;

        public static bool ExecuteQuery(Object obj, string type)
        {
            Type = type;
            Obj = obj;
            if (Type.Equals("Create") || Type.Equals("Update") || Type.Equals("Delete"))
            {
                try
                {
                    Connection = new MySqlConnection(ConfigurationManager.AppSettings["DefaultConnection"]);
                    Connection.Open();
                    Command = Connection.CreateCommand();
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

        public static DataSet ExecuteQuery(Object obj, int qtd)
        {
            Type = "Read";
            Obj = obj;
            Qtd = qtd;
            try
            {
                Connection = new MySqlConnection(ConfigurationManager.AppSettings["DefaultConnection"]);
                Connection.Open();
                Command = Connection.CreateCommand();
                Adapter = new MySqlDataAdapter();
                AddQuery();
                AddParameters();
                DataSet Ds = ExecuteSelect();
                return Ds;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static void AddQuery()
        {
            var props = Obj.GetType().GetProperties();
            var query = "";
            if (Type.Equals("Create"))
            {
                query = "INSERT INTO " + Obj.GetType().Name + "(" + Columns(props) + 
                    ") VALUES(" + Columns(props, "?") + ")";
                Command.CommandText = query;
            }
            else if (Type.Equals("Read"))
            {
                query = "SELECT " + Columns(props) + " FROM " + Obj.GetType().Name;
                Command.CommandText = query;
            }
            else if (Type.Equals("Update"))
            {
                query = "UPDATE " + Obj.GetType().Name + " SET " + Columns(props) + 
                    " WHERE " + props[0].Name + " = " + props[0].GetValue(Obj) + " LIMIT " + Qtd;
                Command.CommandText = query;
            }
            else if (Type.Equals("Delete"))
            {
                query = "DELETE " + Obj.GetType().Name + 
                    " WHERE " + props[0].Name + " = " + props[0].GetValue(Obj);
                Command.CommandText = query;
            }
        }

        private static void AddParameters()
        {
            foreach(PropertyInfo prop in Obj.GetType().GetProperties())
                Command.Parameters.Add(new MySqlParameter(prop.Name, prop.GetValue(Obj)));
        }
        
        private static void Execute()
        {
            Command.ExecuteNonQuery();
            Connection.Close();
            Command.Dispose();
            Connection.Dispose();
        }

        private static DataSet ExecuteSelect()
        {
            var Ds = new DataSet();
            Adapter.SelectCommand = Command;
            Adapter.Fill(Ds);
            Connection.Close();
            Command.Dispose();
            Adapter.Dispose();
            Connection.Dispose();
            return Ds;
        }

        private static string Columns(PropertyInfo[] props)
        {
            var query = "";
            for (int x = 1; x < props.Length; x++)
            {
                query += props[x].Name;
                if (x < props.Length - 1)
                    query += ", ";
            }
            return query;
        }

        private static string Columns(PropertyInfo[] props, string plus)
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
    }
}