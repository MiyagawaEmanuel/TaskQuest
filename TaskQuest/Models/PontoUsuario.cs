using System;

namespace TaskQuest.Models
{
    public class PontoUsuario
    {

        public PontoUsuario()
        {
            Data = DateTime.Now;
        }

        public int UsuarioId { get; set; }

        public int TaskId { get; set; }

        public int Valor { get; set; }

        public DateTime Data { get; set; }

        public virtual Task Task { get; set; }

        public virtual User Usuario { get; set; }
    }
}