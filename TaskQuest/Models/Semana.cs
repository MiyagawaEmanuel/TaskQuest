using System.Collections.Generic;

namespace TaskQuest.Models
{
    public class Semana
    {
        public Semana()
        {
            Quests = new HashSet<Quest>();
        }

        public int Id { get; set; }

        public string Dia { get; set; }

        public string Sigla { get; set; }

        public virtual ICollection<Quest> Quests { get; set; }
    }
}