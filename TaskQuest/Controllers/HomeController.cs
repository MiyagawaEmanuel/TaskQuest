using System.Collections.Generic;
using TaskQuest.Models;
using System.Web.Mvc;
using TaskQuest.ViewModels;
using System.Linq;
using TaskQuest.Identity;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

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
                        if (tsk.Status != 2)
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
            return View();
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
            return View();
        }

    }
}