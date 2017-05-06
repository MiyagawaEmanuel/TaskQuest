namespace TaskQuest.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("feb_feedback")]
    public partial class feb_feedback
    {
        [Key]
        [Column(Order = 0)]
        public int feb_id { get; set; }

        [Required]
        [StringLength(150)]
        public string feb_relatorio { get; set; }

        public int? feb_nota { get; set; }

        [Column("feb_feedback")]
        [StringLength(150)]
        public string feb_feedback1 { get; set; }

        public DateTime feb_data_conclusao { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int tsk_id { get; set; }

        public virtual tsk_task tsk_task { get; set; }
    }
}
