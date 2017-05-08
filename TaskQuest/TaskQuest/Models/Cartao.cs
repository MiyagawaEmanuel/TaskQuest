namespace TaskQuest.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("crt_cartao")]
    public class Cartao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("crt_id")]
        public int Id { get; set; }
        
        [Column("usu_usuario_usu_id")]
        public int UsuarioId { get; set; }

        [Required]
        [Column("crt_bandeira")]
        public string Bandeira { get; set; }

        [Required]
        [Column("crt_numero_cartao")]
        public String Numero { get; set; }

        [Required]
        [Column("crt_nome_titular")]
        public string NomeTitular { get; set; }

        [Required]
        [Column("crt_data_vencimento")]
        public String DataVencimento { get; set; }

        [Required]
        [Column("crt_codigo_seguranca")]
        public string CodigoSeguranca { get; set; }

        public virtual ApplicationUser Usuario { get; set; }
    }
}
