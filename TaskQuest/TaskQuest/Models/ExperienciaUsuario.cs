namespace TaskQuest.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("xpu_experiencia_usuario")]
    public class ExperienciaUsuario
    {
        [Key]
        [Column("usu_id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UsuarioId { get; set; }

        [Key]
        [Column("tsk_id", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TaskId { get; set; }

        [Column("xpg_valor")]
        public int Valor { get; set; }

        [Column("xpu_data")]
        public DateTime Data { get; set; }

        public virtual Task Task { get; set; }

        public virtual ApplicationUser Ususario { get; set; }
    }
}
