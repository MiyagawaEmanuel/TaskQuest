using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TaskQuest.Models
{
    public class Notificacao
    {
        public int Id { get; set; }

        public string Texto { get; set; }

        public string TipoNotificacao { get; set; }

        public string EntidadeModificada { get; set; }

        public DateTime DataNotificacao { get; set; }

        public int GrupoId { get; set; }

        public virtual Grupo Grupo { get; set; }
    }

    public class NotificacaoMetaData
    {

        public NotificacaoMetaData()
        {
            TipoNotificacao = this.GetType();
        }

        public Type GetTipoNotificacao()
        {
            return this.TipoNotificacao;
        }

        private Type TipoNotificacao { get; set; }
    }
}