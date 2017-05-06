namespace TaskQuest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("gru_grupo")]
    public partial class gru_grupo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public gru_grupo()
        {
            qst_quest = new HashSet<qst_quest>();
            msg_mensagem = new HashSet<msg_mensagem>();
            uxg_usuario_grupo = new HashSet<uxg_usuario_grupo>();
            xpg_experiencia_grupo = new HashSet<xpg_experiencia_grupo>();
        }

        [Key]
        public int gru_id { get; set; }

        [Required]
        [StringLength(20)]
        public string gru_nome { get; set; }

        [Required]
        [StringLength(7)]
        public string gru_cor { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime gru_data_criacao { get; set; }

        public bool gru_plano { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<qst_quest> qst_quest { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<msg_mensagem> msg_mensagem { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<uxg_usuario_grupo> uxg_usuario_grupo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<xpg_experiencia_grupo> xpg_experiencia_grupo { get; set; }
    }
}
