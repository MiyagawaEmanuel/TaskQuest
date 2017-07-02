using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskQuest.Models;
using TaskQuest.ViewModels;
using TaskQuest.App_Code;
using System.Data.Entity.Infrastructure;

namespace TaskQuest.Controllers
{
    [Authorize]
    public class QuestController : Controller
    {

        private DbContext db = new DbContext();

        public ActionResult CriarQuest()
        {
            return View("CriarQuest", null);
        }

        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CriarQuest(LinkViewModel model)
        {
            if (ModelState.IsValid)
            {
                return View("CriarQuest", Convert.ToInt32(model.Hash));
            }
            else
            {
                TempData["Alert"] = "Formulário inválido";
                TempData["Class"] = "yellow-alert";
                return RedirectToAction("Inicio", "Home");
            }
        }

        [HttpPost]
        public string CriarQuestPost(QuestViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var quest = new Quest()
                    {
                        Nome = model.Nome,
                        Descricao = model.Descricao,
                        Cor = model.Cor,
                        DataCriacao = DateTime.Now
                    };

                    foreach (var tsk in model.Tasks)
                        quest.Tasks.Add(tsk);

                    if (model.GrupoCriadorId == null)
                        quest.UsuarioCriadorId = User.Identity.GetUserId<int>();
                    else
                        quest.GrupoCriadorId = model.GrupoCriadorId;

                    db.Quest.Add(quest);
                    db.SaveChanges();

                    TempData["Alert"] = "Criado com sucesso";
                    TempData["Class"] = "green-alert";

                    return "true";
                }
                catch
                {
                    return "false";
                }
            }
            else
            {
                return "false";
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LinkViewModel model)
        {

            if (ModelState.IsValid)
            {
                var quest = db.Quest.Find(Convert.ToInt32(model.Hash));
                int user_id = User.Identity.GetUserId<int>();

                if (quest.UsuarioCriadorId == user_id || quest.GrupoCriador.UsuarioGrupos.Where(q => q.UsuarioId == user_id && q.Administrador).Any())
                {
                    return View("QuestAdm", quest.Id);
                }

                return View("Quest", quest.Id);
            }
            else
            {
                TempData["Alert"] = "Formulário inválido";
                TempData["Class"] = "yellow-alert";
                return RedirectToAction("Inicio", "Home");
            }

        }

        [HttpPost]
        public JsonResult GetQuests(string Hash)
        {
            QuestViewModel quest = new QuestViewModel(db.Quest.Find(Convert.ToInt32(Hash)));
            quest.TasksViewModel = new List<TaskViewModel>();
            foreach (var tsk in db.Task.Where(q => q.QuestId == quest.Id).ToList())
            {
                quest.TasksViewModel.Add(new TaskViewModel()
                {
                    Id = tsk.Id,
                    QuestId = tsk.QuestId,
                    Nome = tsk.Nome,
                    Descricao = tsk.Descricao,
                    DataConclusao = tsk.DataConclusao.ToString("yyyy-MM-dd"),
                    Dificuldade = tsk.Dificuldade,
                    Status = tsk.Status
                });


                var aux = db.Feedback.Where(q => q.TaskId == tsk.Id);
                if (aux.Any())
                {
                    var feb = aux.OrderByDescending(q => q.DataConclusao).First();
                    quest.TasksViewModel[quest.TasksViewModel.Count - 1].Feedback = new Feedback()
                    {
                        Id = feb.Id,
                        Nota = feb.Nota,
                        Resposta = feb.Resposta,
                        TaskId = feb.TaskId
                    };
                }

            }

            return Json(quest);
        }

        [HttpPost]
        public string AtualizarQuest(QuestViewModel model)
        {

            if (ModelState.IsValid)
            {
                Quest quest = db.Quest.Find(model.Id);
                quest.Nome = model.Nome;
                quest.Descricao = model.Descricao;
                quest.Cor = model.Cor;
                db.Entry(quest).State = System.Data.Entity.EntityState.Modified;

                foreach (var tsk in model.TasksViewModel)
                {
                    var task = db.Task.Find(tsk.Id);
                    if (task != null)
                    {
                        task.Nome = tsk.Nome;
                        task.Descricao = tsk.Descricao;
                        task.Dificuldade = tsk.Dificuldade;
                        task.Status = tsk.Status;
                        task.DataConclusao = tsk.DataConclusao.StringToDateTime();
                        db.Entry(task).State = System.Data.Entity.EntityState.Modified;

                    }
                    else
                    {
                        Task aux = new Task()
                        {
                            Nome = tsk.Nome,
                            Descricao = tsk.Descricao,
                            Dificuldade = tsk.Dificuldade,
                            Status = tsk.Status,
                            DataConclusao = tsk.DataConclusao.StringToDateTime(),
                            QuestId = model.Id,
                        };
                        db.Task.Add(aux);
                    }

                    if (tsk.Feedback != null)
                    {
                        if (!db.Feedback.Where(q => q.TaskId == tsk.Id).Any())
                        {
                            Feedback feedback = new Feedback()
                            {
                                TaskId = tsk.Id,
                                Resposta = tsk.Feedback.Resposta,
                                Nota = tsk.Feedback.Nota,
                                DataConclusao = DateTime.Now
                            };
                            db.Feedback.Add(feedback);
                        }
                    }
                    else
                        foreach (var feb in db.Feedback.Where(q => q.TaskId == tsk.Id))
                            db.Feedback.Remove(feb);

                }

                foreach (var task in db.Task.Where(q => q.QuestId == model.Id))
                    if (!model.TasksViewModel.Where(q => q.Id == task.Id).Any())
                        db.Task.Remove(task);

                bool saveFailed;
                do
                {
                    saveFailed = false;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        saveFailed = true;

                        // Update original values from the database 
                        var entry = ex.Entries.Single();
                        entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    }

                } while (saveFailed);

                TempData["Alert"] = "Atualizado com sucesso";
                TempData["Class"] = "green-alert";
                return "true";
            }
            else
                return "false";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirQuest(LinkViewModel model)
        {
            db.Quest.Remove(db.Quest.Find(Convert.ToInt32(model.Hash)));
            db.SaveChanges();

            TempData["Alert"] = "Excluído com sucesso";
            TempData["Class"] = "green-alert";

            return RedirectToAction("Inicio", "Home");
        }

        [HttpPost]
        public string MudarStatus(string Id, string Status)
        {

            if (Status != "0" && Status != "1" && Status != "2")
                return "false";

            Task task = db.Task.Find(Convert.ToInt32(Id));
            task.Status = Convert.ToInt32(Status);

            if (task.Status == 1 || task.Status == 2)
                foreach (var feb in db.Feedback.Where(q => q.TaskId == task.Id))
                    db.Feedback.Remove(feb);

            db.SaveChanges();

            return "true";
        }

    }
}