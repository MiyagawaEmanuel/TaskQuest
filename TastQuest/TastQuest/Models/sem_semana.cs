namespace TastQuest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("taskquest.sem_semana")]
    public partial class sem_semana
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sem_semana()
        {
            qst_quest = new HashSet<qst_quest>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int sem_id { get; set; }

        [Required]
        [StringLength(10)]
        public string sem_dia { get; set; }

        [Required]
        [StringLength(1)]
        public string sem_sigla { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<qst_quest> qst_quest { get; set; }
    }
}
