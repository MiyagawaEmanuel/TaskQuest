using System;

namespace TaskQuest.Models
{
    public class ExperienciaGrupo
    {
        public int GrupoId { get; set; }

        public int TaskId { get; set; }

        public int Valor { get; set; }

        public DateTime Data { get; set; }

        public virtual Grupo Grupo { get; set; }

        public virtual Task Task { get; set; }
    }
}