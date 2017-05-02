namespace TastQuest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using TaskQuest.Models;

    [Table("taskquest.qst_quest")]
    public partial class qst_quest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public qst_quest()
        {
            pre_precedencia = new HashSet<pre_precedencia>();
            pre_precedencia1 = new HashSet<pre_precedencia>();
            tsk_task = new HashSet<tsk_task>();
            sem_semana = new HashSet<sem_semana>();
        }

        [Key]
        public int qst_id { get; set; }

        public int? usu_id_criador { get; set; }

        public int? gru_id_criador { get; set; }

        [Required]
        [StringLength(45)]
        public string qst_cor { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime qst_criacao { get; set; }

        [Required]
        [StringLength(45)]
        public string qst_descricao { get; set; }

        [Required]
        [StringLength(45)]
        public string qst_nome { get; set; }

        public virtual gru_grupo gru_grupo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pre_precedencia> pre_precedencia { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pre_precedencia> pre_precedencia1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tsk_task> tsk_task { get; set; }

        public virtual ApplicationUser usu_usuario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sem_semana> sem_semana { get; set; }
    }
}
