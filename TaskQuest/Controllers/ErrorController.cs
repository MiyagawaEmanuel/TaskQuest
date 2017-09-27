using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskQuest.Controllers
{
    public class ErrorController : Controller
    {

        public ActionResult Index()
        {
            return View("Erro404");
        }

        public ViewResult NotFound()
        {
            Response.StatusCode = 404;  //you may want to set this to 200
            return View("Erro404");
        }

    }
}
