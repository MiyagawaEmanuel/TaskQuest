namespace TastQuest.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using TaskQuest.Models;

    [Table("taskquest.crt_cartao")]
    public partial class crt_cartao
    {
        [Key]
        public int crt_id { get; set; }
        
        public int usu_usuario_usu_id { get; set; }

        [Required]
        [StringLength(45)]
        public string crt_bandeira { get; set; }

        public int crt_numero_cartao { get; set; }

        [Required]
        [StringLength(30)]
        public string crt_nome_titular { get; set; }

        [Column(TypeName = "date")]
        public DateTime crt_data_vencimento { get; set; }

        [Required]
        [StringLength(3)]
        public string crt_codigo_seguranca { get; set; }

        public virtual ApplicationUser usu_usuario { get; set; }
    }
}
