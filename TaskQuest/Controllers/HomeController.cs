﻿using System.Collections.Generic;
using TaskQuest.Models;
using System.Web.Mvc;
using TaskQuest.ViewModels;
using System.Linq;
using Microsoft.AspNet.Identity;
using TaskQuest.Identity;
using System.Net;

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
        public ActionResult Inicio()
        {

            User user = db.Users.Find(User.Identity.GetUserId<int>());

            var model = new InicioViewModel();

            model.Grupos.AddRange(user.Grupos.ToList());

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
            List<Quest> model = new List<Quest>();

            User user = db.Users.Find(User.Identity.GetUserId<int>());

            foreach (var gru in user.Grupos)
                foreach (var qst in gru.Quests)
                    model.Add(qst);

            foreach (var qst in user.Quests)
                model.Add(qst);

            model = model.OrderBy(a => a.Tasks.OrderBy(b => b.DataConclusao).FirstOrDefault()).ToList();

            return View(model);
        }

    }
}