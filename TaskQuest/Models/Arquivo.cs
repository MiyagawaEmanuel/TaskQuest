using System;

namespace TaskQuest.Models
{
    public class Arquivo
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Path { get; set; }

        public int Size { get; set; }

        public DateTime UploadDate { get; set; }

        public bool VersaoAtual { get; set; }

        public int TaskId { get; set; }

        public virtual Task Task { get; set; }
    }
}