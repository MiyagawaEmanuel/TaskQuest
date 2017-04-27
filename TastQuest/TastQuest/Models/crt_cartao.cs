namespace TastQuest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("taskquest.crt_cartao")]
    public partial class crt_cartao
    {
        [Key]
        [Column(Order = 0)]
        public int crt_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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

        public virtual usu_usuario usu_usuario { get; set; }
    }
}
