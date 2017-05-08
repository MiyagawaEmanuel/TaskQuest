namespace TaskQuest.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("uxg_usuario_grupo")]
    public class UsuarioGrupo
    {
        [Key]
        [Column(TypeName = "usu_id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UsuarioId { get; set; }

        [Key]
        [Column(TypeName = "gru_id", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GrupoId { get; set; }

        [Column("uxg_administrador")]
        public bool Administrador { get; set; }

        public virtual Grupo Grupo { get; set; }

        public virtual ApplicationUser Usuario { get; set; }
    }
}
