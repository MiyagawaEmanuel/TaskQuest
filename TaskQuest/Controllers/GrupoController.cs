using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public ActionResult CriarGrupo(GrupoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var grupo = model.Update();
                if (grupo != null)
                {
                    var user = db.Users.Find(User.Identity.GetUserId<int>());

                    user.Grupos.Add(grupo);
                    db.SaveChanges();

                    user.Claims.Add(new UserClaim(grupo.Id.ToString(), "Adm"));
                    db.SaveChanges();

                    TempData["Alerta"] = "Criado com sucesso";
                    TempData["Classe"] = "green-alert";

                    return View("Redirect", new RedirectViewModel("/Grupo/Index", Util.Encrypt(grupo.Id.ToString())));
                }
                else
                {
                    TempData["Alerta"] = "Algo deu errado";
                    TempData["Classe"] = "yellow-alert";
                    return RedirectToAction("Inicio", "Home");
                }
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
                int Id;
                if (int.TryParse(Util.Decrypt(model.Hash), out Id))
                {
                    Grupo grupo = db.Grupo.Find(Id);
                    if (!grupo.Users.Any(q => q.Id == User.Identity.GetUserId<int>()))
                    {
                        TempData["Classe"] = "yellow-alert";
                        TempData["Alerta"] = "Você não tem permissão para entrar nessa página";
                        return RedirectToAction("Inicio", "Home");
                    }

                    GrupoViewModel grupoViewModel = new GrupoViewModel(grupo);

                    grupoViewModel.Integrantes.AddRange(grupo.Users);
                    grupoViewModel.Quests.AddRange(grupo.Quests);

                    if (User.Identity.IsAdm(grupo.Id))
                        return View("GrupoAdm", grupoViewModel);

                    return View(grupoViewModel);
                }
                else
                {
                    TempData["Classe"] = "yellow-alert";
                    TempData["Alerta"] = "Algo deu errado";
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
                var grupo = model.Update();
                if (grupo != null)
                {
                    if (!User.Identity.IsAdm(grupo.Id))
                    {
                        TempData["Classe"] = "yellow-alert";
                        TempData["Alerta"] = "Você não tem permissão para realizar esta ação";
                    }
                    else
                    {
                        db.Entry(grupo).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        TempData["Classe"] = "green-alert";
                        TempData["Alerta"] = "Atualizado com sucesso";
                        return View("Redirect", new RedirectViewModel("/Grupo/Index", Util.Encrypt(grupo.Id.ToString())));
                    }
                }
                else
                {
                    TempData["Alerta"] = "Algo deu errado";
                    TempData["Classe"] = "yellow-alert";
                    return RedirectToAction("Inicio", "Home");
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
        public ActionResult AdicionarIntegrante(IntegranteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId<int>());

                int Id;
                if (int.TryParse(Util.Decrypt(model.GrupoId), out Id))
                {
                    Grupo grupo = db.Grupo.Find(Id);
                    if (!User.Identity.IsAdm(grupo.Id))
                    {
                        TempData["Classe"] = "yellow-alert";
                        TempData["Alerta"] = "Você não pode executar esta ação";
                        return View("Redirect", new RedirectViewModel("/Grupo/Index", Util.Encrypt(grupo.Id.ToString())));
                    }

                    var aux2 = db.Users.Where(q => q.Email == model.Email);
                    if (aux2.Any())
                    {
                        User usuario = aux2.First();

                        if (!grupo.Users.Any(q => q == usuario))
                        {
                            grupo.Users.Add(usuario);
                            db.SaveChanges();

                            TempData["Classe"] = "green-alert";
                            TempData["Alerta"] = "Integrante adicionado com sucesso";
                            return View("Redirect", new RedirectViewModel("/Grupo/Index", Util.Encrypt(grupo.Id.ToString())));
                        }
                        
                    }
                    else
                    {
                        TempData["Classe"] = "yellow-alert";
                        TempData["Alerta"] = "Usuário não cadastrado";

                        (new EmailService()).SendAsync(new IdentityMessage() 
                        { 
                            Destination = model.Email,
                            Subject = "",
                            Body = ""
                        });

                        return View("Redirect", new RedirectViewModel("/Grupo/Index", Util.Encrypt(grupo.Id.ToString())));
                    }
                }
            }
            TempData["Classe"] = "yellow-alert";
            TempData["Alerta"] = "Formulário inválido";
            return RedirectToAction("Inicio", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirIntegrante(IntegranteViewModel model)
        {
            if (ModelState.IsValid)
            {
                int Id;
                if (int.TryParse(Util.Decrypt(model.GrupoId), out Id))
                {
                    Grupo grupo = db.Grupo.Find(Id);
                    if (!grupo.Users.Any(q => q.Id == User.Identity.GetUserId<int>()))
                    {
                        TempData["Classe"] = "yellow-alert";
                        TempData["Alerta"] = "Você não tem permissão para entrar nessa página";
                        return RedirectToAction("Inicio", "Home");
                    }

                    var aux2 = grupo.Users.Where(q => q.Email == model.Email);
                    if (aux2.Any())
                    {
                        grupo.Users.Remove(aux2.First());
                        db.SaveChanges();
                        TempData["Classe"] = "green-alert";
                        TempData["Alerta"] = "Removido com sucesso";
                        return View("Redirect", new RedirectViewModel("/Grupo/Index", Util.Encrypt(grupo.Id.ToString())));
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
        public ActionResult TornarAdministrador(IntegranteViewModel model)
        {
            if (ModelState.IsValid)
            {
                int Id;
                if (int.TryParse(Util.Decrypt(model.GrupoId), out Id))
                {
                    var grupo = db.Grupo.Find(Id);
                    if (User.Identity.IsAdm(grupo.Id))
                    {
                        var aux3 = db.Users.ToList().Where(q => q.Email == model.Email);
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

                        return View("Redirect", new RedirectViewModel("/Grupo/Index", Util.Encrypt(grupo.Id.ToString())));

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
                int Id;
                if (int.TryParse(Util.Decrypt(model.Hash), out Id))
                {
                    Grupo grupo = db.Grupo.Find(Id);
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
            int Id;
            if (int.TryParse(Util.Decrypt(model.Hash), out Id))
            {
                var gru = db.Grupo.Find(Id);
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

        [HttpPost]
        public ActionResult GetInfoIntegrante(IntegranteViewModel model)
        {
            int Id;
            if (int.TryParse(Util.Decrypt(model.GrupoId), out Id))
            {
                if (db.Grupo.Find(Id).Users.Any(q => q.Id == User.Identity.GetUserId<int>()))
                {
                    var aux2 = db.Users.ToList().Where(q => q.Email == model.Email);
                    if(aux2.Any())
                    {
                        User user = aux2.First();
                        var telefones = user.Telefones.ToList().Select(q => new { Tipo = q.Tipo, Numero = q.Numero });
                        return Json(new { Success = "true", Nome =  user.Nome + " " + user.Sobrenome, Email = user.Email, Telefones = telefones});
                    }
                    else
                    {
                        return Json(new { Success = "false", Alert = "Algo deu errado" });
                    }
                }
                    
                else
                    return Json(new { Success = "false", Alert = "Você não tem privilérios para realizar esta ação" });
            }
            else
                return Json(new { Success = "false", Alert = "Algo deu errado" });
        }

    }
}