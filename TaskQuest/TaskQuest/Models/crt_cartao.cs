namespace TaskQuest.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("crt_cartao")]
    public partial class crt_cartao
    {
        [Key]
        public int crt_id { get; set; }
        
        public int usu_usuario_usu_id { get; set; }

        [Required]
        public string crt_bandeira { get; set; }

        [Required]
        public String crt_numero_cartao { get; set; }

        [Required]
        public string crt_nome_titular { get; set; }

        [Required]
        public String crt_data_vencimento { get; set; }

        [Required]
        public string crt_codigo_seguranca { get; set; }

        public virtual ApplicationUser usu_usuario { get; set; }
    }
}
