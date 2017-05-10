using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace TaskQuest.App_Code
{
    private var connection = new MySqlConnection(ConfigurationManager.AppSettings["DefaultConnection"]);
    var command = new conenction.CreateCommand();
    
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

    public static class Cursor
    {
        
        public static IDataAdapter Adapter(IDbCommand command)
        {
            IDbDataAdapter adap = new MySqlDataAdapter();
            adap.SelectCommand = command;
            return adap;
        }

    }
}
