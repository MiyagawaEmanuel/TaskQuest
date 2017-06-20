namespace TaskQuest.ViewModels
{
    public class ButtonViewModel
    {

        public ButtonViewModel() { }

        public ButtonViewModel(int id)
        {
            Id = id;
        }

        public string ActionValue { get; set; }

        public int Id { get; set; }

        public string ButtonDescription { get; set; }

    }
}