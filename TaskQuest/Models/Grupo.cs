using System;
using System.Collections.Generic;
using System.Diagnostics;
using TaskQuest.Data;
using System.Linq;

namespace TaskQuest.Models
{
    public class Grupo: NotificacaoMetaData
    {
        public Grupo(): base()
        {
            Quests = new HashSet<Quest>();
            Mensagens = new HashSet<Mensagem>();
            Users = new HashSet<User>();
            DataCriacao = DateTime.Now;
        }

        public int Id { get; set; }

        public string Nome { get; set; }

        public string Cor { get; set; }

        public DateTime DataCriacao { get; set; }

        public string Descricao { get; set; }

        public virtual ICollection<Quest> Quests { get; set; }

        public virtual ICollection<Mensagem> Mensagens { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Notificacao> Notificacoes { get; set; }

        public virtual Pagamento Pagamento { get; set; }

    }
}
