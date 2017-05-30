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
        public ActionResult Index() //V
        {
            return View();
        }

        public ActionResult Login(LoginViewModel model) //V
        {
            return RedirectToAction("Inicio");
        }

        public ActionResult Register(RegisterViewModel model) //V
        {
            return RedirectToAction("Index");
        }
        
        public ActionResult Inicio() //V
        {
            var model = new InicioViewModel();
            return View(model);
        }

        public ActionResult Usuario() //V
        {
            var model = new UsuarioViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Usuario(UsuarioViewModel model) //V
        {
            ViewBag.Result = "200//ok";
            return View();
        }

        [HttpPost]
        public ActionResult AdicionarTelefone(TelefoneViewModel model) //V
        {
            ViewBag.Result = "200/ok";
            return RedirectToAction("Usuario");
        }

        [HttpPost]
        public ActionResult AdicionarCartao(CartaoViewModel model) //V
        {
            ViewBag.Result = "200/ok";
            return RedirectToAction("Usuario");
        }

        public ActionResult Grupos() //V
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

        public ActionResult Grupo() //V
        {
            var model = new GrupoViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Grupo(GrupoViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public ActionResult AdicionarUsuarioGrupo(AdicionarUsuarioGrupoViewModel model) //V
        {
            return RedirectToAction("Grupos");
        }

    }
}