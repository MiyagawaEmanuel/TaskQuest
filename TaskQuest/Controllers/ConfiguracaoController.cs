using Microsoft.AspNet.Identity;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using TaskQuest.Models;
using TaskQuest.ViewModels;

namespace TaskQuest.Controllers
{
    [Authorize]
    public class ConfiguracaoController : Controller
    {

        private DbContext db = new DbContext();

        public ActionResult Index()
        {
            var model = new ConfiguracaoViewModel();
            model.usuario = db.Users.Find(User.Identity.GetUserId<int>());
            model.Cartoes = model.usuario.Cartoes.ToList();
            model.Telefones = model.usuario.Telefones.ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarUsuario(User model)
        {

            if (User.Identity.GetUserId<int>() == model.Id)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["Alert"] = "Atualizado com sucesso";
                TempData["Class"] = "green-alert";
            }
            else
            {
                TempData["Alert"] = "Você não tem permissão para entrar nesta página";
                TempData["Class"] = "yellow-alert";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Configuracao");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarTelefone([Bind(Prefix = "Item2")] Telefone model)
        {

            if (User.Identity.GetUserId<int>() == model.UsuarioId)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["Alert"] = "Atualizado com sucesso";
                TempData["Class"] = "green-alert";
            }
            else
            {
                TempData["Alert"] = "Você não tem permissão para realizar essa ação";
                TempData["Class"] = "yellow-alert";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarCartao([Bind(Prefix = "Item2")] Cartao model)
        {
            if (User.Identity.GetUserId<int>() == model.UsuarioId)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["Alert"] = "Atualizado com sucesso";
                TempData["Class"] = "green-alert";
            }
            else
            {
                TempData["Alert"] = "Você não tem permissão para realizar essa ação";
                TempData["Class"] = "yellow-alert";
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirTelefone([Bind(Prefix = "Item2")] Telefone model)
        {
            if (User.Identity.GetUserId<int>() == model.UsuarioId)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                TempData["Alert"] = "Removido com sucesso";
                TempData["Class"] = "green-alert";
            }
            else
            {
                TempData["Alert"] = "Você não tem permissão para realizar essa ação";
                TempData["Class"] = "yellow-alert";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirCartao([Bind(Prefix = "Item2")] Cartao model)
        {
            if (User.Identity.GetUserId<int>() == model.UsuarioId)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                TempData["Alert"] = "Removido com sucesso";
                TempData["Class"] = "green-alert";
            }
            else
            {
                TempData["Alert"] = "Você não tem permissão para realizar essa ação";
                TempData["Class"] = "yellow-alert";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarTelefone(Telefone model)
        {
            model.UsuarioId = User.Identity.GetUserId<int>();
            db.Telefone.Add(model);
            db.SaveChanges();
            TempData["Alert"] = "Criado com sucesso";
            TempData["Class"] = "green-alert";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarCartao(Cartao model)
        {
            model.UsuarioId = User.Identity.GetUserId<int>();
            db.Cartao.Add(model);
            db.SaveChanges();
            TempData["Alert"] = "Criado com sucesso";
            TempData["Class"] = "green-alert";

            return RedirectToAction("Index");
        }
    }
}