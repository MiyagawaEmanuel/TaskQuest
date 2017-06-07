using System;

namespace TaskQuest.Models
{
    public class Mensagem
    {
        public int Id { get; set; }

        public int UsuarioRemetenteId { get; set; }

        public int? UsuarioDestinatarioId { get; set; }

        public int? GrupoDestinatarioId { get; set; }

        public string Conteudo { get; set; }

        public DateTime Data { get; set; }

        public virtual Grupo GrupoDestinatario { get; set; }

        public virtual User UsuarioRemetente { get; set; }

        public virtual User UsuarioDestinatario { get; set; }
    }
}