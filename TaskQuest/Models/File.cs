using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskQuest.Models
{
    public class File
    {

        public File()
        {
            DataEnvio = DateTime.Now;
            IsValid = false;
        }

        public int Id { get; set; }

        public string Nome { get; set; }

        public string Url { get; set; }

        public string Response { get; set; }

        public Double Size { get; set; }

        public DateTime DataEnvio { get; set; }

        public bool IsValid { get; set; }

        public int? TaskId { get; set; }

        public int UserId { get; set; }

        public virtual Task Task { get; set; }

        public virtual User User { get; set; }
    }
}