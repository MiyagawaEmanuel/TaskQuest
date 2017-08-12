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
            return View("CriarQuest", new LinkViewModel(""));
        }

        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CriarQuest(LinkViewModel model)
        {
            if (ModelState.IsValid)
            {
                return View("CriarQuest", model);
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
                {
                    var aux = db.Grupo.ToList().Where(q => Util.Hash(q.Id.ToString()) == model.GrupoCriadorId);
                    if (aux.Any() && User.Identity.IsAdm(aux.First().Id)
                        quest.GrupoCriadorId = aux.First().Id;
                    else
                        return "false";
                }
                
                db.Quest.Add(quest);
                db.SaveChanges();

                TempData["Alerta"] = "Criado com sucesso";
                TempData["Classe"] = "green-alert";

                return "true";

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
                var aux = db.Users.Find(User.Identity.GetUserId<int>()).Grupo().ToList().Where(q => Util.Hash(q.Id) == model.Hash);
                if(aux.Any())
                {
                    var aux2 = db.Quest.ToList().Where(q => Util.Hash(q.Id.ToString()) == model.Hash);
                    if (aux2.Any())
                    {
                        var quest = aux2.First();
                        int user_id = User.Identity.GetUserId<int>();

                        if (quest.UsuarioCriadorId == user_id || User.Identity.IsAdm(quest.GrupoCriador.Id))
                        {
                            return View("QuestAdm", new LinkViewModel(quest.Id.ToString()));
                        }

                        return View("Quest", new LinkViewModel(quest.Id.ToString()));
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
                    TempData["Alerta"] = "Você não pode entrar nesta página";
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
            var aux = db.Users.Find(User.Identity.GetUserId<int>()).Grupo().ToList().Where(q => Util.Hash(q.Id) == model);
            if(aux.Any())
            {
                var aux2 = db.Quest.ToList().Where(q => Util.Hash(q.Id.ToString()) == Hash);
                if (aux2.Any())
                {
                    QuestViewModel quest = new QuestViewModel(aux2.First());
                    quest.TasksViewModel = new List<TaskViewModel>();
                    foreach (var tsk in db.Task.Where(q => q.QuestId == quest.Id).ToList())
                    {
                        quest.TasksViewModel.Add(new TaskViewModel()
                        {
                            Id = Util.Hash(tsk.Id.ToString()),
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
        }

        [HttpPost]
        public string AtualizarQuest(QuestViewModel model)
        {
            if(ModelState.IsValid)
            {
                Quest quest = db.Quest.Find(model.Id);
                
                if(!db.Users.Find(User.Identity.GetUserId<int>()).Grupos.ToList().Where(q => q.Id == quest.GrupoId) || !User.Identity.GetUserId<int>() == quest.UserId)
                    return "false";
                    
                quest.Nome = model.Nome;
                quest.Descricao = model.Descricao;
                quest.Cor = model.Cor;
                db.Entry(quest).State = System.Data.Entity.EntityState.Modified;

                foreach (var tsk in model.TasksViewModel)
                {

                    Task task;

                    var aux2 = db.Task.ToList().Where(q => Util.Hash(q.Id.ToString()) == tsk.Id);
                    if (aux2.Any())
                    {
                        task = aux2.First();

                        task.Nome = tsk.Nome;
                        task.Descricao = tsk.Descricao;
                        task.Dificuldade = tsk.Dificuldade;
                        task.Status = tsk.Status;
                        task.DataConclusao = tsk.DataConclusao.StringToDateTime();
                        db.Entry(task).State = System.Data.Entity.EntityState.Modified;

                    }
                    else
                    {
                        task = new Task()
                        {
                            Nome = tsk.Nome,
                            Descricao = tsk.Descricao,
                            Dificuldade = tsk.Dificuldade,
                            Status = tsk.Status,
                            DataConclusao = tsk.DataConclusao.StringToDateTime(),
                            QuestId = model.Id,
                        };
                        db.Task.Add(task);
                    }

                    if (tsk.Feedback != null)
                    {
                        if (!db.Feedback.Any(q => q.TaskId == task.Id))
                        {
                            Feedback feedback = new Feedback()
                            {
                                TaskId = task.Id,
                                Resposta = tsk.Feedback.Resposta,
                                Nota = tsk.Feedback.Nota,
                                DataCriacao = DateTime.Now
                            };
                            db.Feedback.Add(feedback);
                        }
                        else
                        {
                            Feedback feedback = task.Feedbacks.First();
                            feedback.Resposta = tsk.Feedback.Resposta;
                            feedback.Nota = tsk.Feedback.Nota;
                            feedback.DataCriacao = DateTime.Now;
                            db.Entry(feedback).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                    else
                        foreach (var feb in db.Feedback.Where(q => q.TaskId == task.Id))
                            db.Feedback.Remove(feb);
                }

                foreach (var task in db.Task.Where(q => q.QuestId == model.Id))
                    if (!model.TasksViewModel.Any(q => q.Id == Util.Hash(task.Id.ToString())))
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
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirQuest(LinkViewModel model)
        {
            if(db.Users.Find().Grupos.ToList().Where().Any() || )
            {
                var aux = db.Quest.ToList().Where(q => Util.Hash(q.Id.ToString()) == model.Hash);
                if (aux.Any())
                {
                    
                    var quest = aux.First();
                    
                    if(!db.Users.Find(User.Identity.GetUserId<int>()).Grupos.ToList().Where(q => q.Id == quest.GrupoId) || !User.Identity.GetUserId<int>() == quest.UserId)
                    {
                        TempData["Alerta"] = "Você não pode executar esta ação";
                        TempData["Classe"] = "yellow-alert";
                    }
                    else
                    {
                        db.Quest.Remove(aux.First());
                        db.SaveChanges();

                        TempData["Alerta"] = "Excluído com sucesso";
                        TempData["Classe"] = "green-alert";
                    }
                }
                else
                {
                    TempData["Alerta"] = "Algo deu errado";
                    TempData["Classe"] = "yellow-alert";
                }
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
                
                if(!db.Users.Find(User.Identity.GetUserId<int>()).Grupos.ToList().Where(q => q.Id == quest.GrupoId) || !User.Identity.GetUserId<int>() == quest.UserId)
                    return "false";
                    
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
