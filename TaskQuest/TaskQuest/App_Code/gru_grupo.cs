using System;

namespace TaskQuest.App_Code
{
    public class gru_grupo
    {

        public gru_grupo()
        {
            gru_data_criacao = DateTime.Now;
        }

        public gru_grupo(string nome, string cor, bool plano)
        {
            gru_nome = nome;
            gru_cor = cor;
            gru_data_criacao = DateTime.Now;
            gru_plano = plano;
        }

        public int gru_id { get; set; }

        public string gru_nome
        {
            get { return gru_nome; }
            set
            {
                if ((value.Length > 0) && (value.Length <= 20))
                    gru_nome = value;
            }
        }

        public string gru_cor
        {
            get { return gru_cor; }
            set
            {
                if ((value.Length > 0) && (value.Length <= 20))
                    gru_cor = value;
            }
        }

        public DateTime gru_data_criacao { get; private set; }

        public bool gru_plano { get; set; }
    }
}
