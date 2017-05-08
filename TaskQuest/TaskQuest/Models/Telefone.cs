namespace TaskQuest.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tel_telefone")]
    public class Telefone
    {
        [Key]
        [Column(TypeName = "tel_id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("usu_id")]
        public int UsuarioId { get; set; }

        [Column("tel_ddd")]
        public int Ddd { get; set; }

        [Column("tel_numero")]
        public int Numero { get; set; }

        [Required]
        [StringLength(15)]
        [Column("tel_tipo")]
        public string Tipo { get; set; }

        public virtual ApplicationUser Usuario { get; set; }
    }
}
