using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace TaskQuest.App_Code
{
    public static class Cursor{
        private var connection = new MySqlConnection(ConfigurationManager.AppSettings["DefaultConnection"]).open();
        private var command = new conenction.CreateCommand();
        private var adapter =  new MySqlDataAdapter();

        public static class AddQuery(string query)
        {
            command.CommandText = query;
        }

        public static class AddParameter(string parametro, object valor)
        {
            command.Parameters.Add(new MySqlParameter(nomeDoParametro, valor););
        }
        
        public static class Execute()
        {

        }

        public static class Select()
        {

        }
    }
}
