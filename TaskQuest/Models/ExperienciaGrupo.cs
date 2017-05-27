using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskQuest.Models
{
    [Table("xpg_experiencia_grupo")]
    public class ExperienciaGrupo
    {
        [Key]
        [Column("gru_id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GrupoId { get; set; }

        [Key]
        [Column("tsk_id", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TaskId { get; set; }

        [Column("xpg_valor")]
        public int Valor { get; set; }

        [Column("xpg_data")]
        public DateTime Data { get; set; }

        public virtual Grupo Grupo { get; set; }

        public virtual Task Task { get; set; }
    }
}