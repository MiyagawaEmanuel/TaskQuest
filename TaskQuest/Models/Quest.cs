using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskQuest.Models
{
    public class Quest
    {
        public Quest()
        {
            Precedencias = new HashSet<Precedencia>();
            Antecedencias = new HashSet<Precedencia>();
            Tasks = new HashSet<Task>();
            Semanas = new HashSet<Semana>();
        }

        public int Id { get; set; }

        public int? UsuarioCriadorId { get; set; }

        public int? GrupoCriadorId { get; set; }

        [Required]
        [StringLength(7, MinimumLength = 4)]
        public string Cor { get; set; }

        public DateTime DataCriacao { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public string Nome { get; set; }

        public virtual Grupo GrupoCriador { get; set; }

        public virtual ICollection<Precedencia> Precedencias { get; set; }

        public virtual ICollection<Precedencia> Antecedencias { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }

        public virtual User UsuarioCriador { get; set; }

        public virtual ICollection<Semana> Semanas { get; set; }
    }
}
