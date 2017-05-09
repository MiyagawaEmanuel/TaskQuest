using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TaskQuest.Models
{
    [Table("tsk_task")]
    public class Task
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Task()
        {
            Feedbacks = new HashSet<Feedback>();
            Precedencias = new HashSet<Precedencia>();
            Antecedencias = new HashSet<Precedencia>();
            ExperienciaUsuarios = new HashSet<ExperienciaUsuario>();
            ExperienciaGrupos = new HashSet<ExperienciaGrupo>();
            Arquivos = new HashSet<Arquivo>();
        }

        [Key]
        [Column("tsk_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("qst_id")]
        public int QuestId { get; set; }

        [Required]
        [StringLength(45)]
        [Column("tsk_nome")]
        public string Nome { get; set; }

        [Column("tsk_status")]
        public int Status { get; set; }

        [Column("tsk_dificuldade")]
        public int Dificuldade { get; set; }

        [Column("tsk_duracao")]
        public TimeSpan Duracao { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("tsk_data_cricacao")]
        public DateTime DataCricacao { get; set; }

        [Column("tsk_data_conclusao")]
        public DateTime? DataConclusao { get; set; }

        [Column("tsk_verificacao")]
        public bool RequerVerificacao { get; set; }

        [Column("usu_id_responsavel")]
        public int UsuarioResponsavelId { get; set; }

        public virtual ApplicationUser UsuarioResponsavel { get; set; }

        public virtual Quest Quest { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feedback> Feedbacks { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Precedencia> Precedencias { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Precedencia> Antecedencias { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExperienciaUsuario> ExperienciaUsuarios { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExperienciaGrupo> ExperienciaGrupos { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Arquivo> Arquivos { get; set; }
    }
}