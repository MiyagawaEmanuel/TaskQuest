namespace TaskQuest.Models
{
    public class Precedencia
    {
        public int Id { get; set; }

        public int? QuestAntecedenteId { get; set; }

        public int? TaskAntecedenteId { get; set; }

        public int? QuestPrecedenteId { get; set; }

        public int? TaskPrecedenteId { get; set; }

        public virtual Quest QuestAntecedente { get; set; }

        public virtual Task TaskAntecedente { get; set; }

        public virtual Quest QuestPrecedente { get; set; }

        public virtual Task TaskPrecedente { get; set; }
    }
}