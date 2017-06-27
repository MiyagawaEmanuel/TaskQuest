using Microsoft.AspNet.Identity;
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
            db.Grupo.Add(model);
            db.SaveChanges();

            UsuarioGrupo uxg = new UsuarioGrupo()
            {
                UsuarioId = User.Identity.GetUserId<int>(),
                GrupoId = model.Id
            };
            db.UsuarioGrupo.Add(uxg);
            db.SaveChanges();

            TempData["Alert"] = "Criado com sucesso";
            TempData["Class"] = "green-alert";

            return RedirectToAction("Grupo", "Home", new ButtonViewModel(model.Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grupo(ButtonViewModel model)
        {

            Grupo grupo = db.Grupo.Find(model.Id);
            UsuarioGrupo usuariogrupo;

            try
            {
                usuariogrupo = grupo.UsuarioGrupos.Where(q => q.UsuarioId == User.Identity.GetUserId<int>()).First();
            }
            catch
            {
                TempData["Class"] = "yellow-alert";
                TempData["Alert"] = "Você não tem permissão para entrar nesta página";
                return RedirectToAction("Grupos");
            }

            GrupoViewModel grupoViewModel = new GrupoViewModel();

            grupoViewModel.Grupo = grupo;

            foreach (var uxg in grupo.UsuarioGrupos)
                grupoViewModel.Integrantes.Add(uxg.Usuario);

            if (usuariogrupo.Administrador)
                return View("GrupoAdmin", model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarGrupo(GrupoViewModel model)
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
            return RedirectToAction("Grupo", new ButtonViewModel(model.Grupo.Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarUsuarioGrupo(AdicionarUsuarioGrupoViewModel model)
        {

            if (db.UsuarioGrupo.Where(q => q.UsuarioId == User.Identity.GetUserId<int>() && q.GrupoId == model.gru_id).First().Administrador)
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
                    GrupoId = model.gru_id,
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
            return RedirectToAction("Grupo", new ButtonViewModel(model.gru_id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirIntegranteGrupo(EditarIntegranteViewModel model) //V
        {

            if (db.UsuarioGrupo.Where(q => q.UsuarioId == User.Identity.GetUserId<int>() && q.GrupoId == model.gru_id).First().Administrador)
            {
                TempData["Class"] = "yellow-alert";
                TempData["Alert"] = "Você não tem permissão para realizar essa ação";
                return RedirectToAction("Inicio", "Home");
            }

            try
            {
                db.Entry(db.UsuarioGrupo.Where(q => q.UsuarioId == model.usu_id && q.GrupoId == model.gru_id).First())
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
            return RedirectToAction("Grupo", new ButtonViewModel(model.gru_id));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TornarAdministrador(EditarIntegranteViewModel model)
        {

            if (db.UsuarioGrupo.Where(q => q.UsuarioId == User.Identity.GetUserId<int>() && q.GrupoId == model.gru_id).First().Administrador)
            {
                TempData["Class"] = "yellow-alert";
                TempData["Alert"] = "Você não tem permissão para realizar essa ação";
                return RedirectToAction("Inicio", "Home");
            }

            try
            {
                var uxg = db.UsuarioGrupo.Where(q => q.UsuarioId == model.usu_id && q.GrupoId == model.gru_id).First();
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
            return RedirectToAction("Grupo", new ButtonViewModel(model.gru_id));
        }

        public ActionResult ExcluirGrupo(ButtonViewModel model)
        {

            if (db.UsuarioGrupo.Where(q => q.UsuarioId == User.Identity.GetUserId<int>() && q.GrupoId == model.Id).First().Administrador)
            {
                TempData["Class"] = "yellow-alert";
                TempData["Alert"] = "Você não tem permissão para realizar essa ação";
                return RedirectToAction("Inicio", "Home");
            }

            try
            {
                db.Entry(db.Grupo.Find(model.Id)).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            catch
            {
                TempData["Class"] = "yellow-alert";
                TempData["Alert"] = "Algo deu errado";
            }

            TempData["Class"] = "green-alert";
            TempData["Alert"] = "Removido com sucesso";
            return RedirectToAction("Grupos", "Home");

        }
    }
}