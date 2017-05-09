using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskQuest.Models
{
    [Table("feb_feedback")]
    public class Feedback
    {
        [Key]
        [Column("feb_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Column("feb_relatorio")]
        public string Relatorio { get; set; }

        [Column("feb_nota")]
        public int? Nota { get; set; }

        [StringLength(150)]
        [Column("feb_feedback")]
        public string Resposta { get; set; }

        [Column("feb_data_conclusao")]
        public DateTime DataConclusao { get; set; }

        [Column("tsk_id")]
        public int TaskId { get; set; }

        [Column("usu_id_responsavel")]
        public int UsuarioResponsavelId { get; set; }

        public virtual Task Task { get; set; }

        public virtual ApplicationUser UsuarioResponsavel { get; set; }
    }
}