namespace TastQuest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("taskquest.usu_usuario")]
    public partial class usu_usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public usu_usuario()
        {
            crt_cartao = new HashSet<crt_cartao>();
            msg_mensagem = new HashSet<msg_mensagem>();
            msg_mensagem1 = new HashSet<msg_mensagem>();
            pre_precedencia = new HashSet<pre_precedencia>();
            qst_quest = new HashSet<qst_quest>();
            tel_telefone = new HashSet<tel_telefone>();
            uxg_usuario_grupo = new HashSet<uxg_usuario_grupo>();
            xpu_experiencia_usuario = new HashSet<xpu_experiencia_usuario>();
        }

        [Key]
        public int usu_id { get; set; }

        [Required]
        [StringLength(20)]
        public string usu_nome { get; set; }

        [Required]
        [StringLength(20)]
        public string usu_sobrenome { get; set; }

        [Required]
        [StringLength(45)]
        public string usu_email { get; set; }

        [Required]
        [StringLength(30)]
        public string usu_senha { get; set; }

        [Required]
        [StringLength(7)]
        public string usu_cor { get; set; }

        public bool usu_confirmado { get; set; }

        [Column(TypeName = "date")]
        public DateTime usu_data_nascimento { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(1)]
        public string usu_sexo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<crt_cartao> crt_cartao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<msg_mensagem> msg_mensagem { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<msg_mensagem> msg_mensagem1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pre_precedencia> pre_precedencia { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<qst_quest> qst_quest { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tel_telefone> tel_telefone { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<uxg_usuario_grupo> uxg_usuario_grupo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<xpu_experiencia_usuario> xpu_experiencia_usuario { get; set; }
    }
}
