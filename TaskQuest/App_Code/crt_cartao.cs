namespace TaskQuest.App_Code
{
    public class crt_cartao
    {

        public crt_cartao
        (
            int crt_id, 
            int usu_id, 
            string crt_numero, 
            string crt_nome_titular, 
            string crt_data_vencimento, 
            string crt_codigo_seguranca
        )
        {
            this.crt_id = crt_id;
            this.usu_id = usu_id;
            this.crt_numero = crt_numero;
            this.crt_nome_titular = crt_nome_titular;
            this.crt_data_vencimento = crt_data_vencimento;
            this.crt_codigo_seguranca = crt_codigo_seguranca;
        }

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