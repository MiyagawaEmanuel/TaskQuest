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
            if(ModelState.IsValid)
            {
                db.Grupo.Add(model);
                db.SaveChanges();

                UsuarioGrupo uxg = new UsuarioGrupo()
                {
                    UsuarioId = User.Identity.GetUserId<int>(),
                    GrupoId = model.Id,
                    Administrador = true
                };
                db.UsuarioGrupo.Add(uxg);
                db.SaveChanges();

                TempData["Alert"] = "Criado com sucesso";
                TempData["Class"] = "green-alert";

                return RedirectToAction("Inicio", "Home");
            }
            else
            {
                TempData["Alert"] = "Formulário Inválido";
                TempData["Class"] = "yellow-alert";
                return RedirectToAction("Inicio", "Home");
            }
        }

        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(LinkViewModel model)
        {

            if(ModelState.IsValid)
            {
                Grupo grupo = db.Grupo.Find(Convert.ToInt32(model.Hash));

                UsuarioGrupo usuariogrupo;

                try
                {
                    usuariogrupo = grupo.UsuarioGrupos.Where(q => q.UsuarioId == User.Identity.GetUserId<int>()).First();
                }
                catch
                {
                    TempData["Class"] = "yellow-alert";
                    TempData["Alert"] = "Você não tem permissão para entrar nessa página ";
                    return RedirectToAction("Inicio", "Home");
                }

                GrupoViewModel grupoViewModel = new GrupoViewModel()
                {
                    Grupo = grupo,
                };

                foreach (var uxg in grupo.UsuarioGrupos)
                    grupoViewModel.Integrantes.Add(new Tuple<bool, User>(uxg.Administrador, uxg.Usuario));

                if (usuariogrupo.Administrador)
                    return View("GrupoAdm", grupoViewModel);

                return View(grupoViewModel);
            }
            else
            {
                TempData["Alert"] = "Algo deu errado";
                TempData["Class"] = "yellow-alert";
                return RedirectToAction("Inicio", "Home");
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarGrupo(GrupoViewModel model)
        {

            if(ModelState.IsValid)
            {
                if (db.UsuarioGrupo.Where(q => q.UsuarioId == User.Identity.GetUserId<int>() && q.GrupoId == model.Grupo.Id).First().Administrador)
                {
                    TempData["Class"] = "yellow-alert";
                    TempData["Alert"] = "Você não tem permissão para realizar essa ação";
                    return RedirectToAction("Inicio", "Home");
                }

                db.Entry(model.Grupo).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                TempData["Class"] = "green-alert";
                TempData["Alert"] = "Atualizado com sucesso";
            }
            else
            {
                TempData["Alert"] = "Algo deu errado";
                TempData["Class"] = "yellow-alert";
                return RedirectToAction("Inicio", "Home");
            }

            return RedirectToAction("Inicio", "Home");
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarIntegrante(AdicionarIntegranteViewModel model)
        {
            
            if(ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId<int>());
                if (!db.UsuarioGrupo.Where(q => q.UsuarioId == user.Id && q.GrupoId == model.GrupoId).First().Administrador)
                {
                    TempData["Class"] = "yellow-alert";
                    TempData["Alert"] = "Você não tem permissão para realizar essa ação";
                    return RedirectToAction("Inicio", "Home");
                }

                try
                {
                    User usuario = db.Users.Where(q => q.Email == model.Email).First();
                    UsuarioGrupo usuarioGrupo = new UsuarioGrupo()
                    {
                        UsuarioId = usuario.Id,
                        GrupoId = model.GrupoId,
                        Administrador = false
                    };
                    db.UsuarioGrupo.Add(usuarioGrupo);
                    db.SaveChanges();
                }
                catch
                {
                    TempData["Class"] = "yellow-alert";
                    TempData["Alert"] = "Usuário não encontrado";
                }

                TempData["Class"] = "green-alert";
                TempData["Alert"] = "Integrante adicionado com sucesso";
            }
            else
            {
                TempData["Alert"] = "Algo deu errado";
                TempData["Class"] = "yellow-alert";
            }

            return RedirectToAction("Inicio", "Home");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirIntegrante(EditarIntegranteViewModel model) 
        {

            if(ModelState.IsValid)
            {
                int UserId = User.Identity.GetUserId<int>();
                if (!db.UsuarioGrupo.Where(q => q.UsuarioId == UserId && q.GrupoId == model.GrupoId).First().Administrador)
                {
                    TempData["Class"] = "yellow-alert";
                    TempData["Alert"] = "Você não tem permissão para realizar essa ação";
                    return RedirectToAction("Inicio", "Home");
                }

                try
                {
                    db.Entry(db.UsuarioGrupo.Where(q => q.UsuarioId == model.UserId && q.GrupoId == model.GrupoId).First())
                    .State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
                catch
                {
                    TempData["Class"] = "yellow-alert";
                    TempData["Alert"] = "Usuário não encontrado";
                }

                TempData["Class"] = "green-alert";
                TempData["Alert"] = "Removido com sucesso";
            }
            else
            {
                TempData["Alert"] = "Formulário inválido";
                TempData["Class"] = "yellow-alert";
            }

            return RedirectToAction("Inicio", "Home");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TornarAdministrador(EditarIntegranteViewModel model)
        {

            if(ModelState.IsValid)
            {
                int UserId = User.Identity.GetUserId<int>();
                if (!db.UsuarioGrupo.Where(q => q.UsuarioId == UserId && q.GrupoId == model.GrupoId).First().Administrador)
                {
                    TempData["Class"] = "yellow-alert";
                    TempData["Alert"] = "Você não tem permissão para realizar essa ação";
                    return RedirectToAction("Inicio", "Home");
                }

                try
                {
                    var uxg = db.UsuarioGrupo.Where(q => q.UsuarioId == model.UserId && q.GrupoId == model.GrupoId).First();
                    uxg.Administrador = true;
                    db.Entry(uxg).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                catch
                {
                    TempData["Class"] = "yellow-alert";
                    TempData["Alert"] = "Algo deu errado";
                }

                TempData["Class"] = "green-alert";
                TempData["Alert"] = "Integrante promovido a Administrador com sucesso";
            }
            else
            {
                TempData["Alert"] = "Algo deu errado";
                TempData["Class"] = "yellow-alert";
            }

            return RedirectToAction("Inicio", "Home");
            
        }

        public ActionResult ExcluirGrupo(LinkViewModel model)
        {

            if(ModelState.IsValid)
            {
                int UserId = User.Identity.GetUserId<int>();
                var grupo = db.Grupo.Find(Convert.ToInt32(model.Hash));
                if (!db.UsuarioGrupo.Where(q => q.UsuarioId == UserId && q.GrupoId == grupo.Id).First().Administrador)
                {
                    TempData["Class"] = "yellow-alert";
                    TempData["Alert"] = "Você não tem permissão para realizar essa ação";
                    return RedirectToAction("Inicio", "Home");
                }

                try
                {
                    db.Grupo.Remove(grupo);
                    db.SaveChanges();
                    TempData["Class"] = "green-alert";
                    TempData["Alert"] = "Removido com sucesso";
                }
                catch
                {
                    TempData["Class"] = "yellow-alert";
                    TempData["Alert"] = "Algo deu errado";
                } 
            }
            else
            {
                TempData["Alert"] = "Algo deu errado";
                TempData["Class"] = "yellow-alert";
            }

            return RedirectToAction("Inicio", "Home");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SairGrupo(LinkViewModel model)
        {

            var UserId = User.Identity.GetUserId<int>();
            var Grupoid = Convert.ToInt32(model.Hash);
            UsuarioGrupo uxg = db.UsuarioGrupo.Where(q => q.UsuarioId == UserId && q.GrupoId == Grupoid).First();

            db.Entry(uxg).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            TempData["Alert"] = "Bem sucedido";
            TempData["Class"] = "green-alert";

            return RedirectToAction("Inicio", "Home");
        }

    }
}