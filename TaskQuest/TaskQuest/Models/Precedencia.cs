namespace TaskQuest.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("pre_precedencia")]
    public class Precedencia
    {
        [Key]
        [Column("pre_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("qst_id_antecedente")]
        public int? QuestAntecedenteId { get; set; }

        [Column("tsk_id_antecedente")]
        public int? TaskAntecedenteId { get; set; }

        [Column("qst_id_precedente")]
        public int? QuestPrecedenteId { get; set; }

        [Column("tsk_id_precedente")]
        public int? TaskPrecedenteId { get; set; }

        [Column("usu_id_responsavel_conclusao")]
        public int ResponsavelId { get; set; }

        public virtual Quest QuestAntecedente { get; set; }

        public virtual Task TaskAntecedente { get; set; }

        public virtual Quest QuestPrecedente { get; set; }

        public virtual Task TaskPrecedente { get; set; }

        public virtual ApplicationUser Responsavel { get; set; }
    }
}
