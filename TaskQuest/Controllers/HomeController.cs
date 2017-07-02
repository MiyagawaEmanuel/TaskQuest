using System.Collections.Generic;
using TaskQuest.Models;
using System.Web.Mvc;
using TaskQuest.ViewModels;
using System.Linq;
using TaskQuest.Identity;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

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
                TempData["Alert"] = "Você não está logado";
                TempData["Class"] = "yellow-alert";
            }
            return View();
        }

        [Authorize]
        public ActionResult Inicio()
        {

            User user = db.Users.Find(User.Identity.GetUserId<int>());

            var model = new InicioViewModel();

            foreach (var uxg in user.UsuarioGrupos)
                foreach (var gru in db.Grupo.Where(q => q.Id == uxg.GrupoId).ToArray())
                    model.Grupos.Add(gru);

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

            return View(model);
        }

        [Authorize]
        public ActionResult Feedbacks()
        {

            List<Feedback> model = new List<Feedback>();
            List<Grupo> grupos = new List<Grupo>();
            List<Task> tasks = new List<Task>();

            User user = db.Users.Find(User.Identity.GetUserId<int>());

            foreach (var uxg in user.UsuarioGrupos)
                foreach (var gru in db.Grupo.Where(q => q.Id == uxg.GrupoId).ToArray())
                    grupos.Add(gru);

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

            return View(model);
        }

        [Authorize]
        public ActionResult Grupos()
        {
            List<Grupo> model = new List<Grupo>();
            foreach (var uxg in db.Users.Find(User.Identity.GetUserId<int>()).UsuarioGrupos)
                model.Add(db.Grupo.Find(uxg.GrupoId));
            return View(model);
        }

        [Authorize]
        public ActionResult Pendencias()
        {
            List<Task> model = new List<Task>();

            User user = db.Users.Find(User.Identity.GetUserId<int>());

            List<Grupo> grupos = new List<Grupo>();
            foreach (var uxg in user.UsuarioGrupos)
                grupos.Add(uxg.Grupo);

            foreach (var gru in grupos)
                foreach (var qst in gru.Quests)
                    foreach (var tsk in qst.Tasks)
                        model.Add(tsk);

            foreach (var qst in user.Quests)
                foreach (var tsk in qst.Tasks)
                    model.Add(tsk);

            return View(model);
        }

    }
}