using System;
using TaskQuest.App_Code;

namespace TaskQuest.Models
{
    public class usu_usuario
    {

        private int _usu_id;
        public int usu_id
        {
            get => _usu_id;
            set
            {
                if (value > 0)
                    _usu_id = value;
            }
        }

        private string _usu_sobrenome;
        public string usu_sobrenome
        {
            get => _usu_sobrenome;
            set
            {
                if (value.Length > 0 && value.Length <= 20)
                    _usu_sobrenome = value;
            }
        }

        private string _usu_data_nascimento;
        public string usu_data_nascimento
        {
            get => _usu_data_nascimento;
            set
            {
                if (value.Length > 0)
                    _usu_data_nascimento = value;
            }
        }

        private string _usu_sexo;
        public string usu_sexo
        {
            get => _usu_sexo;
            set
            {
                if (value.Equals("M", StringComparison.OrdinalIgnoreCase) || value.Equals("F", StringComparison.OrdinalIgnoreCase))
                    _usu_sexo = value;
            }
        }

        private string _usu_email;
        public string usu_email
        {
            get => _usu_email;
            set
            {
                if (value.Length > 0 && value.Length <= 50)
                    _usu_email = value;
            }
        }

        private bool _usu_email_confirmado;
        public bool usu_email_confirmado
        {
            get => _usu_email_confirmado;
            set => _usu_email_confirmado = value;
        }

        private string _usu_senha;
        public string usu_senha
        {
            get => _usu_senha;
            set 
            {
                if (value.Length > 0)
                    _usu_senha = value;
            }
        }

        private string _usu_selo_seguranca;
        public string usu_selo_seguranca
        {
            get => _usu_selo_seguranca;
            set
            {
                if (value.Length > 0)
                    _usu_selo_seguranca = value;
            }
        }

        private bool _usu_dois_passos_login;
        public bool usu_dois_passos_login
        {
            get => _usu_dois_passos_login;
            set => _usu_dois_passos_login = value;
        }

        private DateTime _usu_data_desbloqueio;
        public DateTime usu_data_desbloqueio
        {
            get => _usu_data_desbloqueio;
            set
            {
                if (value.CompareTo(DateTime.Now) > 0)
                    _usu_data_desbloqueio = value;
            }
        }

        private bool _usu_bloqueado;
        public bool usu_bloqueado
        {
            get => _usu_bloqueado;
            set => _usu_bloqueado = value;
        }

        private int _usu_contador_acesso_falho;
        public int usu_contador_acesso_falho
        {
            get => _usu_contador_acesso_falho;
            set
            {
                if (value >= 0)
                    _usu_contador_acesso_falho = value;
            }
        }

        private string _usu_nome;
        public string usu_nome
        {
            get => _usu_nome;
            set
            {
                if (value.Length > 0)
                    _usu_nome = value;
            }
        }
    }
}