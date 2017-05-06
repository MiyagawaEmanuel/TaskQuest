namespace TaskQuest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tsk_task")]
    public partial class tsk_task
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tsk_task()
        {
            feb_feedback = new HashSet<feb_feedback>();
            pre_precedencia = new HashSet<pre_precedencia>();
            pre_precedencia1 = new HashSet<pre_precedencia>();
            xpu_experiencia_usuario = new HashSet<xpu_experiencia_usuario>();
            xpg_experiencia_grupo = new HashSet<xpg_experiencia_grupo>();
            arq_arquivos = new HashSet<arq_arquivos>();
        }

        [Key]
        public int tsk_id { get; set; }

        public int qst_id { get; set; }

        [Required]
        [StringLength(45)]
        public string tsk_nome { get; set; }

        public int tsk_status { get; set; }

        public int tsk_dificuldade { get; set; }

        public TimeSpan? tsk_duracao { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime tsk_data_criacao { get; set; }

        public DateTime? tsk_data_conclusao { get; set; }

        public bool tsk_verificacao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<feb_feedback> feb_feedback { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pre_precedencia> pre_precedencia { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pre_precedencia> pre_precedencia1 { get; set; }

        public virtual qst_quest qst_quest { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<xpu_experiencia_usuario> xpu_experiencia_usuario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<xpg_experiencia_grupo> xpg_experiencia_grupo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<arq_arquivos> arq_arquivos { get; set; }
    }
}
