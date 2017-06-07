using System;
using System.Collections.Generic;

namespace TaskQuest.Models
{
    public class Task
    {
        public Task()
        {
            Feedbacks = new HashSet<Feedback>();
            Precedencias = new HashSet<Precedencia>();
            Antecedencias = new HashSet<Precedencia>();
            ExperienciaUsuarios = new HashSet<ExperienciaUsuario>();
            ExperienciaGrupos = new HashSet<ExperienciaGrupo>();
            Arquivos = new HashSet<Arquivo>();
        }

        public int Id { get; set; }

        public int QuestId { get; set; }

        public string Nome { get; set; }

        public int Status { get; set; }

        public int Dificuldade { get; set; }

        public TimeSpan Duracao { get; set; }
        
        public string Cor { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime? DataConclusao { get; set; }

        public bool RequerVerificacao { get; set; }

        public int? UsuarioResponsavelId { get; set; }

        public virtual User UsuarioResponsavel { get; set; }

        public virtual Quest Quest { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; }

        public virtual ICollection<Precedencia> Precedencias { get; set; }

        public virtual ICollection<Precedencia> Antecedencias { get; set; }

        public virtual ICollection<ExperienciaUsuario> ExperienciaUsuarios { get; set; }

        public virtual ICollection<ExperienciaGrupo> ExperienciaGrupos { get; set; }

        public virtual ICollection<Arquivo> Arquivos { get; set; }
    }
}