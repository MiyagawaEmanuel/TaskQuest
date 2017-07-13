﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using TaskQuest.Identity;
using TaskQuest.Models;
using TaskQuest.ViewModels;

namespace TaskQuest.Controllers
{
    [Authorize]
    public class GrupoController : Controller
    {

        private DbContext db = new DbContext();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CriarGrupo(Grupo model)
        {
            if (ModelState.IsValid)
            {
                db.Grupo.Add(model);
                db.SaveChanges();

                var user = db.Users.Find(User.Identity.GetUserId<int>());
                user.Grupos.Add(model);
                db.SaveChanges();

                user.Claims.Add(new UserClaim(model.Id.ToString(), "Adm"));
                db.SaveChanges();

                TempData["Alerta"] = "Criado com sucesso";
                TempData["Classe"] = "green-alert";

                return RedirectToAction("Inicio", "Home");
            }
            else
            {
                TempData["Alerta"] = "Formulário Inválido";
                TempData["Classe"] = "yellow-alert";
                return RedirectToAction("Inicio", "Home");
            }
        }

        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(LinkViewModel model)
        {
            if (ModelState.IsValid)
            {
                var aux = db.Grupo.ToList().Where(q => Util.Hash(q.Id.ToString()) == model.Hash);
                if (aux.Any())
                {
                    Grupo grupo = aux.First();
                    if (!grupo.Users.Any(q => q.Id == User.Identity.GetUserId<int>()))
                    {
                        TempData["Classe"] = "yellow-alert";
                        TempData["Alerta"] = "Você não tem permissão para entrar nessa página";
                        return RedirectToAction("Inicio", "Home");
                    }

                    GrupoViewModel grupoViewModel = new GrupoViewModel()
                    {
                        Grupo = grupo,
                    };

                    grupoViewModel.Integrantes.AddRange(grupo.Users);

                    if (User.Identity.IsAdm(grupo.Id))
                        return View("GrupoAdm", grupoViewModel);

                    return View(grupoViewModel);
                }
                else
                {
                    TempData["Classe"] = "yellow-alert";
                    TempData["Alerta"] = "Algo deu errado - No select";
                    return RedirectToAction("Inicio", "Home");
                }
            }
            else
            {
                TempData["Alerta"] = "Algo deu errado";
                TempData["Classe"] = "yellow-alert";
                return RedirectToAction("Inicio", "Home");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarGrupo(GrupoViewModel model)
        {

            if (ModelState.IsValid)
            {

                if (!User.Identity.IsAdm(model.Grupo.Id))
                {
                    TempData["Classe"] = "yellow-alert";
                    TempData["Alerta"] = "Você não tem permissão para realizar esta ação";
                }
                else
                {
                    db.Entry(model.Grupo).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    TempData["Classe"] = "green-alert";
                    TempData["Alerta"] = "Atualizado com sucesso";
                }

            }
            else
            {
                TempData["Alerta"] = "Algo deu errado";
                TempData["Classe"] = "yellow-alert";
                return RedirectToAction("Inicio", "Home");
            }

            return RedirectToAction("Inicio", "Home");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarIntegrante(AdicionarIntegranteViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId<int>());

                var aux = db.Grupo.ToList().Where(q => Util.Hash(q.Id.ToString()) == model.GrupoId);
                if (aux.Any())
                {
                    Grupo grupo = aux.First();
                    if (!User.Identity.IsAdm(grupo.Id))
                    {
                        TempData["Classe"] = "yellow-alert";
                        TempData["Alerta"] = "Você não tem permissão para realizar esta ação";
                        return RedirectToAction("Inicio", "Home");
                    }

                    var aux2 = db.Users.Where(q => q.Email == model.Email);
                    if (aux2.Any())
                    {
                        User usuario = aux2.First();

                        grupo.Users.Add(usuario);
                        db.SaveChanges();

                        TempData["Classe"] = "green-alert";
                        TempData["Alerta"] = "Integrante adicionado com sucesso";
                    }
                    else
                    {
                        TempData["Classe"] = "yellow-alert";
                        TempData["Alerta"] = "Usuário não encontrado";
                    }
                }
            }
            else
            {
                TempData["Alerta"] = "Algo deu errado";
                TempData["Classe"] = "yellow-alert";
            }

            return RedirectToAction("Inicio", "Home");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirIntegrante(EditarIntegranteViewModel model)
        {

            if (ModelState.IsValid)
            {
                var aux = db.Grupo.ToList().Where(q => Util.Hash(q.Id.ToString()) == model.GrupoId);
                if (aux.Any())
                {
                    Grupo grupo = aux.First();
                    if (!grupo.Users.Any(q => q.Id == User.Identity.GetUserId<int>()))
                    {
                        TempData["Classe"] = "yellow-alert";
                        TempData["Alerta"] = "Você não tem permissão para entrar nessa página";
                        return RedirectToAction("Inicio", "Home");
                    }

                    var aux2 = grupo.Users.Where(q => Util.Hash(q.Id.ToString()) == model.UserId);
                    if (aux2.Any())
                    {
                        grupo.Users.Remove(aux2.First());
                        TempData["Classe"] = "green-alert";
                        TempData["Alerta"] = "Removido com sucesso";
                    }
                }
            }
            else
            {
                TempData["Alerta"] = "Algo deu errado";
                TempData["Classe"] = "yellow-alert";
            }

            return RedirectToAction("Inicio", "Home");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TornarAdministrador(EditarIntegranteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var aux = db.Grupo.ToList().Where(q => Util.Hash(q.Id.ToString()) == model.GrupoId);
                if (aux.Any())
                {
                    var grupo = aux.First();
                    if (User.Identity.IsAdm(grupo.Id))
                    {
                        var aux3 = db.Users.ToList().Where(q => Util.Hash(q.Id.ToString()) == model.UserId);
                        if (aux3.Any())
                        {
                            aux3.First().Claims.Add(new UserClaim(grupo.Id.ToString(), "Adm"));

                            TempData["Classe"] = "green-alert";
                            TempData["Alerta"] = "Integrante promovido a Administrador com sucesso";

                            db.Entry(aux3.First()).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            TempData["Classe"] = "green-alert";
                            TempData["Alerta"] = "Algo deu errado";
                        }
                    }
                    else
                    {
                        TempData["Classe"] = "yellow-alert";
                        TempData["Alerta"] = "Você não tem permissão para executar essa ação";
                    }
                }
                else
                {
                    TempData["Classe"] = "green-alert";
                    TempData["Alerta"] = "Algo deu errado";
                }
            }
            else
            {
                TempData["Alerta"] = "Algo deu errado";
                TempData["Classe"] = "yellow-alert";
            }

            return RedirectToAction("Inicio", "Home");

        }

        public ActionResult ExcluirGrupo(LinkViewModel model)
        {

            if (ModelState.IsValid)
            {
                var aux = db.Grupo.ToList().Where(q => Util.Hash(q.Id.ToString()) == model.Hash);
                if (aux.Any())
                {
                    Grupo grupo = aux.First();
                    if (!User.Identity.IsAdm(grupo.Id))
                    {
                        TempData["Classe"] = "yellow-alert";
                        TempData["Alerta"] = "Você não tem permissão para executar essa ação";
                        return RedirectToAction("Inicio", "Home");
                    }

                    foreach (var usu in grupo.Users)
                    {
                        var user = User as ClaimsPrincipal;
                    }
                    
                    db.Grupo.Remove(grupo);

                    foreach (var user in grupo.Users)
                        foreach (var claim in user.Claims.ToList())
                            if (claim.ClaimType == "3")
                                db.Entry(claim).State = System.Data.Entity.EntityState.Deleted;
                    
                    db.SaveChanges();

                    TempData["Classe"] = "green-alert";
                    TempData["Alerta"] = "Removido com sucesso";
                }
                else
                {
                    TempData["Alerta"] = "Algo deu errado";
                    TempData["Classe"] = "yellow-alert";
                }
            }
            else
            {
                TempData["Alerta"] = "Algo deu errado";
                TempData["Classe"] = "yellow-alert";
            }

            return RedirectToAction("Inicio", "Home");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SairGrupo(LinkViewModel model)
        {
            var aux = db.Grupo.ToList().Where(q => Util.Hash(q.Id.ToString()) == model.Hash);
            if (aux.Any())
            {
                var gru = aux.First();
                var aux2 = db.Users.Find(User.Identity.GetUserId<int>()).Grupos.Where(q => q.Id == gru.Id);
                if (aux2.Any())
                {
                    Grupo grupo = aux2.First();
                    db.Users.Find(User.Identity.GetUserId<int>()).Grupos.Remove(grupo);
                    db.SaveChanges();
                    TempData["Alerta"] = "Bem sucedido";
                    TempData["Classe"] = "green-alert";
                }
                else
                {
                    TempData["Alerta"] = "Grupo não encontrado";
                    TempData["Classe"] = "yellow-alert";
                }
            }
            else
            {
                TempData["Alerta"] = "Algo deu errado";
                TempData["Classe"] = "yellow-alert";
            }

            return RedirectToAction("Inicio", "Home");
        }

    }
}