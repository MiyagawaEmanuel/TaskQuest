namespace Teste
{
    public class crt_cartao
    {
        
        private int _crt_id;
        public int crt_id
        {
            get => _crt_id;
            set
            {
                if (value > 0)
                    _crt_id = value;
            }
        }

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

        private string _crt_bandeira;
        public string crt_bandeira
        {
            get => _crt_bandeira;
            set
            {
                if (value.Length != 0)
                    _crt_bandeira = value;
            }
        }

        private string _crt_numero;
        public string crt_numero
        {
            get => _crt_numero;
            set
            {
                if (value.Length != 0)
                    _crt_numero = value;
            }
        }

        private string _crt_nome_titular;
        public string crt_nome_titular
        {
            get => _crt_nome_titular;
            set
            {
                if (value.Length != 0)
                    _crt_nome_titular = value;
            }
        }

        private string _crt_data_vencimento;
        public string crt_data_vencimento
        {
            get => _crt_data_vencimento;
            set
            {
                if (value.Length != 0)
                    _crt_data_vencimento = value;
            }
        }

        private string _crt_codigo_seguranca;
    public string crt_codigo_seguranca
    {
        get => _crt_codigo_seguranca;
        set
        {
            if (value.Length != 0)
                _crt_codigo_seguranca = value;
        }
    }

    }
}