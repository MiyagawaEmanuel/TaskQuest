using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskQuest.Models;

namespace TaskQuest.ViewModels
{
    public class FileViewModel
    {

        public FileViewModel() { }

        public FileViewModel(File file)
        {
            Id = Util.Encrypt(file.Id.ToString());
            Nome = file.Nome;
            Url = file.Url;
            Response = file.Response;
            TaskId = Util.Encrypt(file.TaskId.ToString());
            Size = file.Size;
        }

        public string Id { get; set; }

        public string Nome { get; set; }

        public string Url { get; set; }

        public string Response { get; set; }

        public Double Size { get; set; }

        public string TaskId { get; set; }
    }
}