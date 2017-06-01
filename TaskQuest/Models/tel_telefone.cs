namespace TaskQuest.Models
{
    public class tel_telefone
    {
        
        private int _tel_id;
        public int tel_id
        {
            get => tel_id;
            set
            {
                if (value > 0)
                    _tel_id = value;
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

        private int _tel_ddd;
        public int tel_ddd
        {
            get => _tel_ddd;
            set
            {
                if (value > 0)
                    _tel_ddd = value;
            }
        }

        private int _tel_numero_;
        public int tel_numero
        {
            get => _tel_numero_;
            set
            {
                if (value > 0)
                    _tel_numero_ = value;
            }
        }

        private string _tel_tipo;
        public string tel_tipo
        {
            get => _tel_tipo;
            set
            {
                if (value.Length > 0)
                    _tel_tipo = value;
            }
        }

    }
}