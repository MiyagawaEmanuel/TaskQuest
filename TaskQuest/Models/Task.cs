using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using TaskQuest.Data;
using System.Linq;

namespace TaskQuest.Models
{
    public class Task: NotificacaoMetaData
    {
        public Task(): base()
        {
            Feedbacks = new HashSet<Feedback>();
            PontoUsuarios = new HashSet<PontoUsuario>();
            Files = new HashSet<File>();
            DataCriacao = DateTime.Now;
        }

        public int Id { get; set; }

        public int QuestId { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public int Status { get; set; }

        public int Dificuldade { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataConclusao { get; set; }

        public bool RequerVerificacao { get; set; }

        public int? UsuarioResponsavelId { get; set; }

        public virtual User UsuarioResponsavel { get; set; }

        public virtual Quest Quest { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; }

        public virtual ICollection<PontoUsuario> PontoUsuarios { get; set; }

        public virtual ICollection<File> Files { get; set; }
    }
}
