namespace TaskQuest.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tel_telefone")]
    public partial class tel_telefone
    {
        [Key]
        [Column(Order = 0)]
        public int tel_id { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int usu_id { get; set; }

        public int tel_ddd { get; set; }

        public int tel_numero { get; set; }

        [Required]
        [StringLength(15)]
        public string tel_tipo { get; set; }

        public virtual ApplicationUser usu_usuario { get; set; }
    }
}
