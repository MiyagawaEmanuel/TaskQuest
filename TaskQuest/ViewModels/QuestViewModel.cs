using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using TaskQuest.Models;

namespace TaskQuest.ViewModels
{

    public class QuestInfoViewModel
    {

        public string QuestId { get; set; }

        public bool HasGroup { get; set; }

        public string GrupoId { get; set; }

        public List<User> Colaboradores { get; set; }

    }

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
        [StringLength(40, MinimumLength = 1)]
        public string Nome { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 1)]
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
        public TaskViewModel()
        {
            Files = new List<FileViewModel>();
        }

        public Task CriarTask()
        {
            return new Task()
            {
                QuestId = QuestId,
                Nome = Nome,
                Descricao = Descricao,
                Status = Status,
                Dificuldade = Dificuldade,
                DataCriacao = DateTime.Now,
                DataConclusao = DataConclusao
            };
        }

        public string Id { get; set; }

        public int QuestId { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Nome { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 2)]
        public string Descricao { get; set; }

        [Required]
        public DateTime DataConclusao { get; set; }

        [Required]
        [Range(0, 2)]
        public int Status { get; set; }

        [Required]
        [Range(0, 4)]
        public int Dificuldade { get; set; }

        public string UsuarioResponsavelId { get; set; }

        public string ResponsavelNome { get; set; }

        public List<FileViewModel> Files { get; set; }

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
        [Range(0, 5)]
        public int Nota { get; set; }

        [StringLength(300, MinimumLength = 2)]
        public string Relatorio { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 2)]
        public string Resposta { get; set; }

        public string UsuarioResponsavelNome { get; set; }

    }

}