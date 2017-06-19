namespace Teste
{
    public class uxg_usuario_grupo
    {

        public uxg_usuario_grupo() { }

        public uxg_usuario_grupo(int usu_id, int gru_id, bool uxg_administrador)
        {
            this.usu_id = usu_id;
            this.gru_id = gru_id;
            this.uxg_administrador = uxg_administrador;
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

        private bool _uxg_administrador;
        public bool uxg_administrador { get => _uxg_administrador; set => _uxg_administrador = value; }

    }
}