namespace TaskQuest.ViewModels
{
    public class LinkViewModel
    { 

        public LinkViewModel() { }

        public LinkViewModel(string Hash)
        {
            this.Hash = Hash;
        }

        public LinkViewModel(string Id, string Hash, string Action)
        {
            this.Id = Id;
            this.Hash = Hash;
            this.Action = Action;
        }

        public string Id { get; set; }

        public string Hash { get; set; }

        public string Action { get; set; }

    }
}