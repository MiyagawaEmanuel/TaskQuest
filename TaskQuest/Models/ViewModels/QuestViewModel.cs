using System.Collections.Generic;
using System.Linq;

namespace TaskQuest.Models.ViewModels
{
    public class QuestViewModel
    {

        public QuestViewModel() { }

        public QuestViewModel(Quest quest)
        {
            Nome = quest.Nome;
            Descricao = quest.Descricao;
            Cor = quest.Cor;
            Tasks = quest.Tasks.ToList();
        }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public string Cor { get; set; }

        public List<Task> Tasks { get; set; }

        public int? GrupoCriadorId { get; set; }

    }
}