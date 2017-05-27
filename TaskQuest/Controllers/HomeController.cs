using System;
using System.Collections.Generic;
using TaskQuest.App_Code;
using System.Web.Mvc;

namespace TaskQuest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string aux = "";
            List<gru_grupo> grupos = Cursor.Select<gru_grupo>();

            grupos = Quick.Sort(grupos, Comparer<gru_grupo>.Create((x, y) => String.Compare(x.gru_nome, y.gru_nome, true)));
            
            try
            {
                foreach (var grupo in grupos)
                    aux += grupo.gru_nome + ", ";
            }
            catch(Exception e)
            {
                aux = e.ToString();
            }
            return Content(aux);
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
    }
}