using System.Collections.Generic;
using TaskQuest.Models;
using System.Web.Mvc;
using TaskQuest.ViewModels;
using System.Linq;
using Microsoft.AspNet.Identity;
using TaskQuest.Identity;
using System.Net;
using TaskQuest.Data;
using System.Diagnostics;
using TaskQuest;
using System;

namespace TaskQuest.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private DbContext db = new DbContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Notificacao()
        {
            var user = db.Users.Find(User.Identity.GetUserId<int>());
            List<Notificacao> model = new List<Notificacao>();

            foreach (var grupo in user.Grupos)
                foreach (var notificacao in grupo.Notificacoes)
                    model.Add(notificacao);

            model = model.OrderBy(e => e.DataNotificacao).ToList();

            return PartialView("_NotificacaoPartial", model);
        }

        [Authorize]
        public ActionResult Inicio()
        {

            User user = db.Users.Find(User.Identity.GetUserId<int>());

            var model = new InicioViewModel();

            model.Grupos = user.Grupos.ToList();

            foreach (var gru in model.Grupos)
                foreach (var qst in gru.Quests)
                    foreach (var tsk in qst.Tasks)
                        model.Tasks.Add(tsk);

            foreach (var qst in user.Quests)
                foreach (var tsk in qst.Tasks)
                    model.Tasks.Add(tsk);

            foreach (var tsk in model.Tasks)
                foreach (var feb in tsk.Feedbacks)
                    model.Feedbacks.Add(feb);

            model.Grupos = model.Grupos.OrderBy(a => a.Nome).ToList();
            model.Tasks = model.Tasks.OrderBy(a => a.DataConclusao).ToList();
            model.Feedbacks = model.Feedbacks.OrderByDescending(a => a.DataCriacao).ToList();

            return View(model);
        }

        [Authorize]
        public ActionResult Feedbacks()
        {

            List<Feedback> model = new List<Feedback>();
            List<Task> tasks = new List<Task>();

            User user = db.Users.Find(User.Identity.GetUserId<int>());

            foreach (var gru in user.Grupos)
                foreach (var qst in gru.Quests)
                    foreach (var tsk in qst.Tasks)
                        tasks.Add(tsk);

            foreach (var qst in user.Quests)
                foreach (var tsk in qst.Tasks)
                    tasks.Add(tsk);

            foreach (var tsk in tasks)
                foreach (var feb in tsk.Feedbacks)
                    model.Add(feb);

            model = model.OrderBy(a => a.DataCriacao).ToList();

            return View(model);
        }

        [Authorize]
        public ActionResult Grupos()
        {
            List<Grupo> model = new List<Grupo>();

            model.AddRange(db.Users.Find(User.Identity.GetUserId<int>()).Grupos.ToList());

            model = model.OrderBy(a => a.Nome).ToList();

            return View(model);
        }

        [Authorize]
        public ActionResult Quests()
        {
            QuestsViewModel model = new QuestsViewModel();

            User user = db.Users.Find(User.Identity.GetUserId<int>());

            var x = 0;
            foreach (var quest in user.Quests)
            {
                model.Quests.Add(quest);
                foreach (var task in quest.Tasks)
                    model.Tasks.Add(new Tuple<int, Task>(x, task));
                x++;
            }

            foreach (var grupo in user.Grupos)
            {
                foreach (var quest in grupo.Quests)
                {
                    model.Quests.Add(quest);
                    foreach (var task in quest.Tasks)
                        model.Tasks.Add(new Tuple<int, Task>(x, task));
                    x++;
                }
            }

            return View(model);
        }

    }
}