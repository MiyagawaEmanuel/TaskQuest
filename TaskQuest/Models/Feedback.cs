using System;
using System.ComponentModel.DataAnnotations;

namespace TaskQuest.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        public string Relatorio { get; set; }

        public int Nota { get; set; }

        public string Resposta { get; set; }

        public DateTime DataCriacao { get; set; }

        public int TaskId { get; set; }

        public int? UsuarioResponsavelId { get; set; }

        public virtual Task Task { get; set; }

        public virtual User UsuarioResponsavel { get; set; }
    }
}