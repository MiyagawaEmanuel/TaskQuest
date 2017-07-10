using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace TaskQuest.ViewModels
{
    public class LinkViewModel
    { 

        public LinkViewModel() { }

        public LinkViewModel(string Hash)
        {
            this.Hash = Util.Hash(Hash);
        }

        public LinkViewModel(string Id, string Hash, string Action)
        {
            this.Id = Id;
            this.Hash = Util.Hash(Hash);
            this.Action = Action;
        }

        public string Id { get; set; }


        [Required]
        public string Hash { get; set; }

        public string Action { get; set; }

    }
}