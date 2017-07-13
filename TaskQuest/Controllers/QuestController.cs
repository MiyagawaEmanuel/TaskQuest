using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TaskQuest.Models;
using TaskQuest.ViewModels;
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
                return View("CriarQuest", model.Hash);
            }
            else
            {
                TempData["Alerta"] = "Formulário inválido";
                TempData["Classe"] = "yellow-alert";
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

                    TempData["Alerta"] = "Criado com sucesso";
                    TempData["Classe"] = "green-alert";

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
                var aux = db.Quest.ToList().Where(q => Util.Hash(q.Id.ToString()) == model.Hash);
                if (aux.Any())
                {
                    var quest = aux.First();
                    int user_id = User.Identity.GetUserId<int>();

                    if (quest.UsuarioCriadorId == user_id || User.Identity.IsAdm(quest.GrupoCriador.Id))
                    {
                        return View("QuestAdm", Util.Hash(quest.Id.ToString()));
                    }

                    return View("Quest", Util.Hash(quest.Id.ToString()));
                }
                else
                {
                    TempData["Alerta"] = "Algo deu errado";
                    TempData["Classe"] = "yellow-alert";
                    return RedirectToAction("Inicio", "Home");
                }
            }
            else
            {
                TempData["Alerta"] = "Formulário inválido";
                TempData["Classe"] = "yellow-alert";
                return RedirectToAction("Inicio", "Home");
            }

        }

        [HttpPost]
        public JsonResult GetQuests(string Hash)
        {
            var aux = db.Quest.ToList().Where(q => Util.Hash(q.Id.ToString()) == Hash);
            if (aux.Any())
            {
                QuestViewModel quest = new QuestViewModel(aux.First());
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


                    var aux2 = db.Feedback.Where(q => q.TaskId == tsk.Id);
                    if (aux2.Any())
                    {
                        var feb = aux2.OrderByDescending(q => q.DataCriacao).First();
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
            else
            {
                return null;
            }
        }

        [HttpPost]
        public string AtualizarQuest(QuestViewModel model)
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
                            DataCriacao = DateTime.Now
                        };
                        db.Feedback.Add(feedback);
                    }
                    else
                    {
                        Feedback feedback = db.Feedback.Find(tsk.Feedback.Id);
                        feedback.Resposta = tsk.Feedback.Resposta;
                        feedback.Nota = tsk.Feedback.Nota;
                        feedback.DataCriacao = DateTime.Now;
                        db.Entry(feedback).State = System.Data.Entity.EntityState.Modified;
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

            TempData["Alerta"] = "Atualizado com sucesso";
            TempData["Classe"] = "green-alert";
            return "true";

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirQuest(LinkViewModel model)
        {
            var aux = db.Quest.ToList().Where(q => Util.Hash(q.Id.ToString()) == model.Hash);
            if (aux.Any())
            {
                db.Quest.Remove(aux.First());
                db.SaveChanges();

                TempData["Alerta"] = "Excluído com sucesso";
                TempData["Classe"] = "green-alert";
            }
            else
            {
                TempData["Alerta"] = "Algo deu errado";
                TempData["Classe"] = "green-alert";
            }
            return RedirectToAction("Inicio", "Home");
        }

        [HttpPost]
        public string MudarStatus(string Id, string Status)
        {

            if (Status != "0" && Status != "1" && Status != "2")
                return "false";

            var aux = db.Task.ToList().Where(q => Util.Hash(q.Id.ToString()) == Id);
            if (aux.Any())
            {
                Task task = aux.First();
                task.Status = Convert.ToInt32(Status);

                if (task.Status == 1 || task.Status == 2)
                    foreach (var feb in db.Feedback.Where(q => q.TaskId == task.Id))
                        db.Feedback.Remove(feb);

                db.SaveChanges();

                return "true";
            }
            return "false";
        }

    }
}