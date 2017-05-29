namespace TaskQuest.Models
{
    public class UsuarioGrupo
    {
        public int UsuarioId { get; set; }

        public int GrupoId { get; set; }

        public bool Administrador { get; set; }

        public virtual Grupo Grupo { get; set; }

        public virtual User Usuario { get; set; }
    }
}