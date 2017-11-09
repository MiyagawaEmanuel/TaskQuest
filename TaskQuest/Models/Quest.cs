using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using TaskQuest.Data;
using System.Linq;

namespace TaskQuest.Models
{
    public class Quest: NotificacaoMetaData
    {
        public Quest(): base()
        {
            Tasks = new HashSet<Task>();
            DataCriacao = DateTime.Now;
        }

        public int Id { get; set; }

        public int? UsuarioCriadorId { get; set; }

        public int? GrupoCriadorId { get; set; }

        public string Cor { get; set; }

        public DateTime DataCriacao { get; set; }

        public string Descricao { get; set; }

        public string Nome { get; set; }

        public virtual Grupo GrupoCriador { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }

        public virtual User UsuarioCriador { get; set; }
    }
}
