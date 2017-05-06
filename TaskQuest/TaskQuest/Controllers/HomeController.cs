using System.Web.Mvc;
using TaskQuest.Models;

namespace TaskQuest.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}
