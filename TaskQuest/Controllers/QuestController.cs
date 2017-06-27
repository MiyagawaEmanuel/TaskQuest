using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskQuest.Controllers
{
    [Authorize]
    public class QuestController : Controller
    {
        // GET: Quest
        public ActionResult Index()
        {
            return View();
        }
    }
}