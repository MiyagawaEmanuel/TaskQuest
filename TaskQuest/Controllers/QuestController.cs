using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TaskQuest.Models;
using TaskQuest.ViewModels;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using Newtonsoft.Json;
using TaskQuest.Data;

namespace TaskQuest.Controllers
{
    [Authorize]
    public class QuestController : Controller
    {

        private DbContext db = new DbContext();

        public ActionResult CriarQuest()
        {
            return View("CriarQuest", new QuestInfoViewModel() { HasGroup = false });
        }

        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CriarQuest(LinkViewModel model)
        {
            if (ModelState.IsValid)
            {
                int id;
                if (int.TryParse(Util.Decrypt(model.Hash), out id))
                {
                    var grupo = db.Grupo.Find(id);
                    return View("CriarQuest", new QuestInfoViewModel()
                    {
                        HasGroup = true,
                        GrupoId = model.Hash,
                        Colaboradores = grupo.Users.ToList()
                    });
                }

            }
            TempData["Alerta"] = "Formulário inválido";
            TempData["Classe"] = "yellow-alert";
            return RedirectToAction("Inicio", "Home");
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
                    Cor = model.Cor
                };

                if (model.TasksViewModel != null)
                {
                    foreach (var tsk in model.TasksViewModel)
                    {

                        var task = tsk.CriarTask();
                        quest.Tasks.Add(task);

                        if (tsk.UsuarioResponsavelId != null)
                        {
                            int Id;
                            if (int.TryParse(Util.Decrypt(tsk.UsuarioResponsavelId), out Id))
                                task.UsuarioResponsavelId = Id;
                            else
                                return "Algo deu errado";
                        }

                        foreach (var fileViewModel in tsk.Files)
                        {
                            int Id;
                            if (int.TryParse(Util.Decrypt(fileViewModel.Id), out Id))
                            {
                                var file = db.File.Find(Id);
                                task.Files.Add(file);
                                file.IsValid = true;
                                db.Entry(file).State = System.Data.Entity.EntityState.Modified;
                            }
                            else
                                return "Algo deu errado";
                        }
                    }
                }

                if (model.GrupoCriadorId == null)
                    quest.UsuarioCriadorId = User.Identity.GetUserId<int>();
                else
                {
                    int Id;
                    if (int.TryParse(Util.Decrypt(model.GrupoCriadorId), out Id))
                        if (User.Identity.IsAdm(Id))
                            quest.GrupoCriadorId = Id;
                        else
                            return "Você não pode executar esta ação";
                    else
                        return "Formlário inválido";
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
                if (User.Identity.HasQuest(model.Hash))
                {
                    int Id;
                    if (int.TryParse(Util.Decrypt(model.Hash), out Id))
                    {
                        var quest = db.Quest.Find(Id);
                        if (quest.UsuarioCriadorId == User.Identity.GetUserId<int>() || User.Identity.IsAdm(quest.GrupoCriador.Id))
                            return View("QuestAdm", new QuestInfoViewModel()
                            {
                                HasGroup = quest.GrupoCriadorId != null,
                                QuestId = Util.Encrypt(quest.Id.ToString()),
                                Colaboradores = (quest.GrupoCriadorId != null) ? quest.GrupoCriador.Users.ToList() : null
                            });
                        else
                            return View("Quest", new QuestInfoViewModel()
                            {
                                HasGroup = quest.GrupoCriadorId != null,
                                QuestId = Util.Encrypt(quest.Id.ToString())
                            });
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
        public ActionResult GetQuests(string Hash)
        {

            if (User.Identity.HasQuest(Hash))
            {
                int Id;
                if (int.TryParse(Util.Decrypt(Hash), out Id))
                {
                    QuestViewModel quest = new QuestViewModel(db.Quest.Find(Id));
                    quest.TasksViewModel = new List<TaskViewModel>();
                    foreach (var tsk in db.Task.Where(q => q.QuestId == Id).ToList())
                    {

                        List<FileViewModel> files = new List<FileViewModel>();
                        foreach (var file in tsk.Files)
                            if (file.IsValid)
                                files.Add(new FileViewModel(file));

                        quest.TasksViewModel.Add(new TaskViewModel()
                        {
                            Id = Util.Encrypt(tsk.Id.ToString()),
                            QuestId = tsk.QuestId,
                            Nome = tsk.Nome,
                            Descricao = tsk.Descricao,
                            DataConclusao = tsk.DataConclusao,
                            Dificuldade = tsk.Dificuldade,
                            Status = tsk.Status,
                            UsuarioResponsavelId = (tsk.UsuarioResponsavel != null) ? Util.Encrypt(tsk.UsuarioResponsavelId.ToString()) : "",
                            ResponsavelNome = (tsk.UsuarioResponsavel != null) ? tsk.UsuarioResponsavel.Nome + " " + tsk.UsuarioResponsavel.Sobrenome : "",
                            Files = files,
                        });

                        if (tsk.Feedbacks.Count != 0)
                        {
                            var feb = tsk.Feedbacks.OrderByDescending(q => q.DataCriacao).First();
                            quest.TasksViewModel[quest.TasksViewModel.Count - 1].FeedbackViewModel = new FeedbackViewModel(feb);
                        }
                    }
                    return Json(new { data = quest });
                }
            }
            return null;
        }

        [HttpPost]
        public string AtualizarQuest(QuestViewModel quest)
        {

            if (ModelState.IsValid)
            {
                int Id;
                if (int.TryParse(Util.Decrypt(quest.Id), out Id))
                {
                    if (!User.Identity.HasQuest(quest.Id))
                        return "Você não pode executar esta ação";

                    var qst = db.Quest.Find(Id);
                    qst.Nome = quest.Nome;
                    qst.Descricao = quest.Descricao;
                    qst.Cor = quest.Cor;
                    db.Entry(qst).State = System.Data.Entity.EntityState.Modified;

                    foreach (var tsk in quest.TasksViewModel)
                    {

                        Task task;
                        if (int.TryParse(Util.Decrypt(tsk.Id), out Id))
                        {
                            task = db.Task.Find(Id);

                            task.Nome = tsk.Nome;
                            task.Descricao = tsk.Descricao;
                            task.Dificuldade = tsk.Dificuldade;

                            if (task.Status == 0)
                            {
                                if (tsk.Status == 1)
                                    task.DataInicio = DateTime.Now;
                                else if (tsk.Status == 2)
                                {
                                    task.DataInicio = DateTime.Now;
                                    task.DataConclusao = DateTime.Now;
                                }
                            }
                            else if (task.Status == 1)
                                if (tsk.Status == 2)
                                    task.DataConclusao = DateTime.Now;

                            task.Status = tsk.Status;

                            task.DataConclusao = tsk.DataConclusao;

                            if (tsk.UsuarioResponsavelId != null)
                                if (int.TryParse(Util.Decrypt(tsk.UsuarioResponsavelId), out Id))
                                    task.UsuarioResponsavelId = Id;
                                else
                                    return "Formulário inválido";
                            else
                                task.UsuarioResponsavelId = null;

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
                                DataConclusao = tsk.DataConclusao,
                                QuestId = qst.Id,
                            };
                            db.Task.Add(task);
                        }

                        foreach (var file in tsk.Files)
                        {
                            if (int.TryParse(Util.Decrypt(file.Id), out Id))
                            {
                                var fle = db.File.Find(Id);
                                fle.TaskId = task.Id;
                                fle.IsValid = true;
                                db.Entry(fle).State = System.Data.Entity.EntityState.Modified;
                            }
                            else
                                return "Algo deu errado";
                        }

                        if (tsk.FeedbackViewModel != null)
                        {
                            if (!db.Feedback.Any(q => q.TaskId == task.Id))
                            {
                                Feedback feedback = new Feedback()
                                {
                                    TaskId = task.Id,
                                    Resposta = tsk.FeedbackViewModel.Resposta,
                                    Nota = tsk.FeedbackViewModel.Nota,
                                    DataCriacao = DateTime.Now
                                };
                                db.Feedback.Add(feedback);

                                if (qst.GrupoCriador != null)
                                {
                                    if (task.UsuarioResponsavel == null)
                                    {
                                        foreach (var user in qst.GrupoCriador.Users)
                                        {
                                            db.PontoUsuario.Add(new PontoUsuario()
                                            {
                                                Task = task,
                                                Usuario = user,
                                                Valor = feedback.Nota * (task.Dificuldade + 1)
                                            });
                                        }
                                    }
                                    else
                                    {
                                        db.PontoUsuario.Add(new PontoUsuario()
                                        {
                                            Task = task,
                                            Usuario = task.UsuarioResponsavel,
                                            Valor = feedback.Nota * (task.Dificuldade + 1)
                                        });
                                    }
                                }

                            }
                            else
                            {
                                Feedback feedback = task.Feedbacks.First();
                                feedback.Resposta = tsk.FeedbackViewModel.Resposta;
                                feedback.Nota = tsk.FeedbackViewModel.Nota;
                                feedback.DataCriacao = DateTime.Now;
                                db.Entry(feedback).State = System.Data.Entity.EntityState.Modified;

                                if (qst.GrupoCriador != null)
                                {
                                    if (task.UsuarioResponsavel == null)
                                    {
                                        foreach (var user in qst.GrupoCriador.Users)
                                        {
                                            var exp = db.PontoUsuario.Find(task.Id, user.Id);
                                            exp.Valor = feedback.Nota * (task.Dificuldade + 1);
                                            db.Entry(exp).State = System.Data.Entity.EntityState.Modified;
                                        }
                                    }
                                    else
                                    {
                                        var exp = db.PontoUsuario.Find(task.Id, task.UsuarioResponsavelId);
                                        exp.Valor = feedback.Nota * (task.Dificuldade + 1);
                                        db.Entry(exp).State = System.Data.Entity.EntityState.Modified;

                                    }
                                }
                            }
                        }
                        else
                            foreach (var feb in db.Feedback.Where(q => q.TaskId == task.Id))
                                db.Feedback.Remove(feb);
                    }

                    foreach (var task in db.Task.Where(q => q.QuestId == qst.Id))
                        if (!quest.TasksViewModel.Any(q => q.Id == Util.Encrypt(task.Id.ToString())))
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
                else
                    return "Algo deu errado";
            }
            return "Formulário inválido";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirQuest(LinkViewModel model)
        {
            int Id;
            if (int.TryParse(Util.Decrypt(model.Hash), out Id))
            {

                var quest = db.Quest.Find(Id);

                if (!User.Identity.HasQuest(model.Hash))
                {
                    TempData["Alerta"] = "Você não pode executar esta ação";
                    TempData["Classe"] = "yellow-alert";
                }
                else
                {
                    db.Quest.Remove(quest);
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

            return RedirectToAction("Inicio", "Home");
        }

        [HttpPost]
        public string MudarStatus(string Id, string Status)
        {

            if (Status != "0" && Status != "1" && Status != "2")
                return "false";

            int TaskId;
            if (int.TryParse(Util.Decrypt(Id), out TaskId))
            {
                Task task = db.Task.Find(TaskId);

                if (!User.Identity.HasQuest(Util.Encrypt(task.QuestId.ToString())))
                    return "false";

                int status = Convert.ToInt32(Status);
                if (status == 0 || status == 1 || status == 2)
                {

                    if (task.Status == 0)
                    {
                        if (status == 1)
                            task.DataInicio = DateTime.Now;
                        else if (status == 2)
                        {
                            task.DataInicio = DateTime.Now;
                            task.DataConclusao = DateTime.Now;
                        }
                    }
                    else if (task.Status == 1)
                        if (status == 2)
                            task.DataConclusao = DateTime.Now;

                    task.Status = status;

                }
                else
                    return "false";

                task.Status = Convert.ToInt32(Status);

                if (task.Status == 0 || task.Status == 1)
                {
                    foreach (var feb in db.Feedback.Where(q => q.Task.Id == task.Id))
                        db.Feedback.Remove(feb);

                    foreach (var exp in db.PontoUsuario.Where(q => q.Task.Id == task.Id))
                        db.PontoUsuario.Remove(exp);
                }

                db.SaveChanges();

                return "true";
            }
            return "false";
        }

    }
}