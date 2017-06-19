﻿using System;

namespace TaskQuest.Models
{
    public class gru_grupo
    {

        public gru_grupo()
        {
            gru_data_criacao = DateTime.Now;
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

        
        private DateTime _gru_data_criacao;
        public DateTime gru_data_criacao
        {
            get => _gru_data_criacao;  
            set => _gru_data_criacao = value; 
        }

        private bool _gru_plano;
        public bool gru_plano
        {
            get => _gru_plano;
            set => _gru_plano = value;
        }

        private string _gru_descricao;
        public string gru_descricao
        {
            get => _gru_descricao;
            set
            {
                if (value.Length > 0 && value.Length <= 120)
                    _gru_descricao = value;
            }
        }
    }
}