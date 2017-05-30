using System;
using System.Collections.Generic;
using TaskQuest.App_Code;
using TaskQuest.Models;
using System.Web.Mvc;
using TaskQuest.ViewModels;

namespace TaskQuest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(LoginViewModel model)
        {
            return RedirectToAction("Inicio");
        }

        public ActionResult Register(RegisterViewModel model)
        {
            return RedirectToAction("Index");
        }
        
        public ActionResult Inicio()
        {
            var model = new InicioViewModel();
            return View(model);
        }

        public ActionResult Usuario()
        {
            var model = new UsuarioViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Usuario(UsuarioViewModel model)
        {
            ViewBag.Result = "200//ok";
            return View();
        }

        public ActionResult Grupos()
        {
            var model = new GruposViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult CriarGrupo(CriarGrupoViewModel model)
        {
            ViewBag.Result = "200//ok";
            return RedirectToAction("Grupos");
        }

        public ActionResult Grupo()
        {
            var model = new GrupoViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Grupo(GrupoViewModel model)
        {
            return View(model);
        }

    }
}