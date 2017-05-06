namespace TaskQuest.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("xpg_experiencia_grupo")]
    public partial class xpg_experiencia_grupo
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int gru_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int tsk_id { get; set; }

        public int xpg_valor { get; set; }

        public DateTime xpg_data { get; set; }

        public virtual gru_grupo gru_grupo { get; set; }

        public virtual tsk_task tsk_task { get; set; }
    }
}
