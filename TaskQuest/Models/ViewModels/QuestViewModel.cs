using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using TaskQuest.Models;

namespace TaskQuest.ViewModels
{
    public class QuestViewModel
    {

        public QuestViewModel() { }

        public QuestViewModel(Quest quest)
        {
            Id = quest.Id;
            Nome = quest.Nome;
            Descricao = quest.Descricao;
            Cor = quest.Cor;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Nome { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Descricao { get; set; }

        [Required]
        [StringLength(7, MinimumLength = 4)]
        public string Cor { get; set; }

        public List<TaskViewModel> TasksViewModel { get; set; }

        public List<Task> Tasks { get; set; }

        public string GrupoCriadorId { get; set; }

    }

    public class TaskViewModel
    {

        public string Id { get; set; }

        public int QuestId { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public string DataConclusao { get; set; }

        public int Status { get; set; }

        public int Dificuldade { get; set; }

        public Feedback Feedback { get; set; }

    }


}