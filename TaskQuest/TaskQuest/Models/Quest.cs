namespace TaskQuest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("qst_quest")]
    public class Quest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Quest()
        {
            Precedencias = new HashSet<Precedencia>();
            Antecedencias = new HashSet<Precedencia>();
            Tasks = new HashSet<Task>();
            Semanas = new HashSet<Semana>();
        }

        [Key]
        [Column("qst_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("usu_id_criador")]
        public int? UsuarioCriadorId { get; set; }

        [Column("gru_id_criador")]
        public int? GrupoCriadorId { get; set; }

        [Required]
        [StringLength(45)]
        [Column("qst_cor")]
        public string Cor { get; set; }
        
        [Column("qst_criacao")]
        public DateTime DataCricao { get; set; }

        [Required]
        [StringLength(45)]
        [Column("qst_descricao")]
        public string Descricao { get; set; }

        [Required]
        [StringLength(45)]
        [Column("qst_nome")]
        public string Nome { get; set; }

        public virtual Grupo GrupoCriador { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Precedencia> Precedencias { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Precedencia> Antecedencias { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task> Tasks { get; set; }

        public virtual ApplicationUser UsuarioCriador { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Semana> Semanas { get; set; }
    }
}
