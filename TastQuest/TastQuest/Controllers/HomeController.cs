using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TastQuest.Models;
using TastQuest.App_Code;

namespace TastQuest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Grupos(int id)
        {
            using (var db = new TaskQuestModels())
            {
                var UsuarioGrupos = db.uxg_usuario_grupo.Where(uxg => uxg.usu_id == id).ToList();
                
                ViewBag.Grupos = new List<gru_grupo>();
                foreach (uxg_usuario_grupo uxg in UsuarioGrupos)
                {
                    ViewBag.Grupos.Add(db.gru_grupo.Find(uxg.gru_id));
                }

                ViewBag.Grupos = Utilities.Sort(ViewBag.Grupos);

                return View();

            }
        }
    }
}