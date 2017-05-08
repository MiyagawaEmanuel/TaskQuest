namespace TaskQuest.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("msg_mensagem")]
    public class Mensagem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("msg_id")]
        public int Id { get; set; }

        [Column("usu_id_remetente")]
        public int UsuarioRemetenteId { get; set; }

        [Column("usu_id_destinatario")]
        public int? UsuarioDestinatarioId { get; set; }

        [Column("gru_id_destinatario")]
        public int? GrupoDestinatarioId { get; set; }

        [Required]
        [StringLength(120)]
        [Column("msg_conteudo")]
        public string Conteudo { get; set; }
        
        [Column("msg_data")]
        public DateTime Data { get; set; }
        
        public virtual Grupo GrupoDestinatario { get; set; }

        public virtual ApplicationUser UsuarioRemetente { get; set; }

        public virtual ApplicationUser UsuarioDestinatario { get; set; }
    }
}
