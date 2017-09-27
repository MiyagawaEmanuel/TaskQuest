using System.Collections.Generic;
using TaskQuest.Models;
using System.Web.Mvc;
using TaskQuest.ViewModels;
using System.Linq;
using Microsoft.AspNet.Identity;
using TaskQuest.Identity;

namespace TaskQuest.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private DbContext db = new DbContext();

        [AllowAnonymous]
        public ActionResult Index(string returnUrl = null)
        {
            if (returnUrl != null)
            {
                TempData["Alerta"] = "Você não está logado";
                TempData["Classe"] = "yellow-alert";
            }

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
                        model.Pendencias.Add(tsk);

            foreach (var qst in user.Quests)
                foreach (var tsk in qst.Tasks)
                    model.Pendencias.Add(tsk);

            foreach (var tsk in model.Pendencias)
                foreach (var feb in tsk.Feedbacks)
                    model.Feedbacks.Add(feb);
            
            model.Grupos.OrderBy(a => a.Nome);
            model.Pendencias.OrderByDescending(a => a.DataConclusao);
            model.Feedbacks.OrderByDescending(a => a.DataCriacao);

            return View(model);
        }

        [Authorize]
        public ActionResult Feedbacks()
        {

            List<Feedback> model = new List<Feedback>();
            List<Grupo> grupos = new List<Grupo>();
            List<Task> tasks = new List<Task>();

            User user = db.Users.Find(User.Identity.GetUserId<int>());

            grupos.Concat(user.Grupos.ToList());

            foreach (var gru in grupos)
                foreach (var qst in gru.Quests)
                    foreach (var tsk in qst.Tasks)
                        tasks.Add(tsk);

            foreach (var qst in user.Quests)
                foreach (var tsk in qst.Tasks)
                    tasks.Add(tsk);

            foreach (var tsk in tasks)
                foreach (var feb in tsk.Feedbacks)
                    model.Add(feb);

            model.OrderBy(a => a.DataCriacao);

            return View(model);
        }

        [Authorize]
        public ActionResult Grupos()
        {
            List<Grupo> model = new List<Grupo>();

            model.AddRange(db.Users.Find(User.Identity.GetUserId<int>()).Grupos.ToList());

            model.OrderBy(a => a.Nome);

            return View(model);
        }

        [Authorize]
        public ActionResult Pendencias()
        {
            List<Task> model = new List<Task>();

            User user = db.Users.Find(User.Identity.GetUserId<int>());

            foreach (var gru in user.Grupos)
                foreach (var qst in gru.Quests)
                    foreach (var tsk in qst.Tasks)
                        model.Add(tsk);

            foreach (var qst in user.Quests)
                foreach (var tsk in qst.Tasks)
                    model.Add(tsk);

            model.OrderByDescending(a => a.DataConclusao);

            return View(model);
        }

    }
}