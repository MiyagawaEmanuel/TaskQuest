using System;
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
            Id = Util.Encrypt(quest.Id.ToString());
            Nome = quest.Nome;
            Descricao = quest.Descricao;
            Cor = quest.Cor;
        }

        public string Id { get; set; }

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

        public Task CriarTask()
        {
            return new Task()
            {
                QuestId = QuestId,
                Nome = Nome,
                Descricao = Descricao,
                Status = Status,
                Dificuldade = Dificuldade
            };
        }

        public string Id { get; set; }

        public int QuestId { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Nome { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Descricao { get; set; }

        [Required]
        public string DataConclusao { get; set; }

        [Required]
        [Range(0, 2)]
        public int Status { get; set; }

        [Required]
        [Range(0, 4)]
        public int Dificuldade { get; set; }

        public FeedbackViewModel FeedbackViewModel { get; set; }

    }

    public class FeedbackViewModel
    {

        public FeedbackViewModel() { }

        public FeedbackViewModel(Feedback feedback) 
        {
            Id = Util.Encrypt(feedback.Id.ToString());
            Nota = feedback.Nota;
            Relatorio = feedback.Relatorio;
            Resposta = feedback.Resposta;
            UsuarioResponsavelNome = feedback.UsuarioResponsavel != null ? feedback.UsuarioResponsavel.Nome : "";
        }

        public string Id { get; set; }

        [Required]
        [Range(0, 4)]
        public int Nota { get; set; }

        public string Relatorio { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Resposta { get; set; }

        public string UsuarioResponsavelNome { get; set; }

    }

}