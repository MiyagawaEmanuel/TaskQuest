using System;
using System.Collections.Generic;

namespace TaskQuest.Models
{
    public class Grupo
    {
        public Grupo()
        {
            Quests = new HashSet<Quest>();
            Mensagens = new HashSet<Mensagem>();
            UsuarioGrupos = new HashSet<UsuarioGrupo>();
            ExperienciaGrupos = new HashSet<ExperienciaGrupo>();
            DataCriacao = DateTime.Now;
        }

        public int Id { get; set; }

        public string Nome { get; set; }

        public string Cor { get; set; }

        public DateTime DataCriacao { get; private set; }

        public bool Plano { get; set; }

        public virtual ICollection<Quest> Quests { get; set; }

        public virtual ICollection<Mensagem> Mensagens { get; set; }

        public virtual ICollection<UsuarioGrupo> UsuarioGrupos { get; set; }

        public virtual ICollection<ExperienciaGrupo> ExperienciaGrupos { get; set; }
    }
}
