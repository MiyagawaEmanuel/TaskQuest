using TaskQuest.Models;
using TaskQuest.App_Code;
using System;

namespace TaskQuest.App_Code{
    public static class CRUD{
        
        public void Create(Grupo grupo)
        {
            try{
                PropertyInfo attributes = grupo.GetType().GetProperties();
                string sql = "INSERT INTO gru_grupo(gru_nome, gru_cor, gru_plano) VALUES(?gru_nome, ?gru_cor, ?gru_plano)";
            }
            catch(Exception e){}
        }
        
        public void Read(int quntity){}
        
        public void Update(int id){}
        
        public void Delete(int id){}
        
    }
}
