using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace TaskQuest.ViewModels
{
    public class LinkViewModel
    { 

        public LinkViewModel() { }

        public LinkViewModel(string Hash, bool requireHashing=true)
        {
            if (requireHashing)
                this.Hash = Util.Encrypt(Hash);
            else
                this.Hash = Hash;
        }

        public LinkViewModel(string Id, string Hash, string Action, bool requireHashing=true)
        {
            this.Id = Id;

            if (requireHashing)
                this.Hash = Util.Encrypt(Hash);
            else
                this.Hash = Hash;

            this.Action = Action;
        }

        public string Id { get; set; }


        [Required]
        public string Hash { get; set; }

        public string Action { get; set; }

    }
}