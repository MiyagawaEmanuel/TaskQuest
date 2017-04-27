namespace TastQuest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("taskquest.msg_mensagem")]
    public partial class msg_mensagem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int msg_id { get; set; }

        public int usu_id_remetente { get; set; }

        public int? usu_id_destinatario { get; set; }

        public int? gru_id_destinatario { get; set; }

        [Required]
        [StringLength(120)]
        public string msg_conteudo { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime msg_data { get; set; }

        public virtual gru_grupo gru_grupo { get; set; }

        public virtual usu_usuario usu_usuario { get; set; }

        public virtual usu_usuario usu_usuario1 { get; set; }
    }
}
