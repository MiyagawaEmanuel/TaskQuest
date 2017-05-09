using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TaskQuest.Models
{
    [Table("gru_grupo")]
    public class Grupo
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Grupo()
        {
            Quests = new HashSet<Quest>();
            Mensagens = new HashSet<Mensagem>();
            UsuarioGrupos = new HashSet<UsuarioGrupo>();
            ExperienciaGrupos = new HashSet<ExperienciaGrupo>();
        }

        [Key]
        [Column("gru_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Column("gru_nome")]
        public string Nome { get; set; }

        [Required]
        [StringLength(7)]
        [Column("gru_cor")]
        public string Cor { get; set; }

        [Column("gru_data_cricao")]
        public DateTime DataCriacao { get; set; }

        [Column("gru_plano")]
        public bool Plano { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Quest> Quests { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mensagem> Mensagens { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsuarioGrupo> UsuarioGrupos { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExperienciaGrupo> ExperienciaGrupos { get; set; }
    }
}