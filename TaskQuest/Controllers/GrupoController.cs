using Microsoft.AspNet.Identity;
using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
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
                var grupos = db.Grupo.ToList().Select(q => new { Grupo = q, HashId = Util.Hash(q.Id.ToString()) });
                var aux = grupos.Where(q => q.HashId == model.Hash);
                if (aux.Any())
                {
                    Grupo grupo = aux.First().Grupo;
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

                    var aux2 = grupo.Users.Where(q => q.Id == User.Identity.GetUserId<int>());
                    if (aux2.Any())
                        if (aux2.First().Claims.Any(q => q.ClaimType == model.Hash && q.ClaimValue == "Adm"))
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

                if (!model.Grupo.Users.Any(q => q.Id == User.Identity.GetUserId<int>()))
                {
                    TempData["Classe"] = "yellow-alert";
                    TempData["Alerta"] = "Você não tem permissão para entrar nessa página ";
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

                Grupo grupo = db.Grupo.Find(Convert.ToInt32(model.GrupoId));
                if (!grupo.Users.Any(q => q.Id == User.Identity.GetUserId<int>()))
                {
                    TempData["Classe"] = "yellow-alert";
                    TempData["Alerta"] = "Você não tem permissão para entrar nessa página";
                    return RedirectToAction("Inicio", "Home");
                }

                if (!user.Claims.Any(q => q.ClaimType == model.GrupoId.ToString() && q.ClaimValue == "Adm"))
                {
                    TempData["Classe"] = "yellow-alert";
                    TempData["Alerta"] = "Você não tem permissão para realizar esta ação";
                    return RedirectToAction("Inicio", "Home");
                }

                var aux = db.Users.Where(q => q.Email == model.Email);
                if (aux.Any())
                {
                    User usuario = aux.First();

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
                Grupo grupo = db.Grupo.Find(Convert.ToInt32(model.GrupoId));
                if (!grupo.Users.Any(q => q.Id == User.Identity.GetUserId<int>()))
                {
                    TempData["Classe"] = "yellow-alert";
                    TempData["Alerta"] = "Você não tem permissão para entrar nessa página";
                    return RedirectToAction("Inicio", "Home");
                }

                var aux = grupo.Users.Where(q => q.Id == model.UserId);
                if (aux.Any())
                {
                    grupo.Users.Remove(aux.First());
                    TempData["Classe"] = "green-alert";
                    TempData["Alerta"] = "Removido com sucesso";
                }
                
            }
            else
            {
                TempData["Alerta"] = "Formulário inválido";
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
                if (!db.Grupo.Find(Convert.ToInt32(model.GrupoId)).Users.Any(q => q.Id == User.Identity.GetUserId<int>()))
                {
                    TempData["Classe"] = "yellow-alert";
                    TempData["Alerta"] = "Você não tem permissão para entrar nessa página";
                    return RedirectToAction("Inicio", "Home");
                }

                db.Users.Find(model.UserId).Claims.Add(new UserClaim(model.GrupoId.ToString(), "Adm"));

                TempData["Classe"] = "green-alert";
                TempData["Alerta"] = "Integrante promovido a Administrador com sucesso";
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
                var grupos = db.Grupo.ToList().Select(q => new { Grupo = q, HashId = Util.Hash(q.Id.ToString()) });
                var aux = grupos.Where(q => q.HashId == model.Hash);
                if (aux.Any())
                {
                    Grupo grupo = aux.First().Grupo;
                    if (!grupo.Users.Any(q => q.Id == User.Identity.GetUserId<int>()))
                    {
                        TempData["Classe"] = "yellow-alert";
                        TempData["Alerta"] = "Você não tem permissão para entrar nessa página";
                        return RedirectToAction("Inicio", "Home");
                    }

                    db.Grupo.Remove(grupo);
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
            var grupos = db.Grupo.ToList().Select(q => new { Grupo = q, HashId = Util.Hash(q.Id.ToString()) });
            var aux = grupos.Where(q => q.HashId == model.Hash);
            if (aux.Any())
            {
                var gru = aux.First().Grupo;
                var aux2 = db.Users.Find(User.Identity.GetUserId<int>()).Grupos.Where(q => q.Id == gru.Id);
                if (aux2.Any())
                {
                    Grupo grupo = aux2.First();
                    db.Users.Find(User.Identity.GetUserId<int>()).Grupos.Remove(grupo);
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