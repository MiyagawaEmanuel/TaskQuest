using System;

namespace TaskQuest.App_Code
{
    public class gru_grupo
    {

        public gru_grupo()
        {
            gru_data_criacao = DateTime.Now;
        }

        public gru_grupo(int id, string nome, string cor, bool plano)
        {
            gru_id = id;
            gru_nome = nome;
            gru_cor = cor;
            gru_data_criacao = DateTime.Now;
            gru_plano = plano;
        }

        private int _gru_id;
        public int gru_id
        {
            get => _gru_id;
            set
            {
                if (value > 0)
                    _gru_id = value;
            }
        }

        private string _gru_nome;
        public string gru_nome
        {
            get => _gru_nome;
            set
            {
                if (value.Length > 0 && value.Length <= 20)
                    _gru_nome = value;
            }
        }

        private string _gru_cor;
        public string gru_cor
        {
            get => _gru_cor;
            set
            {
                if (value.Length > 0 && value.Length <= 20)
                    _gru_cor = value;
            }
        }

        private DateTime _gru_data_criacao;
        public DateTime gru_data_criacao
        {
            get => _gru_data_criacao;  
            set => _gru_data_criacao = DateTime.Now; 
        }

        private bool _gru_plano;
        public bool gru_plano
        {
            get => _gru_plano;
            set => _gru_plano = value;
        }
    }
}
