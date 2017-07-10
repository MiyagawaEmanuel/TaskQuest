using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskQuest.Identity;
using TaskQuest.Models;
using TaskQuest.ViewModels;
using System.Data.Entity.Infrastructure;

namespace TaskQuest.Controllers
{
    [Authorize]
    public class ConfiguracaoController : Controller
    {
        
        private ApplicationUserManager _userManager;

        public ConfiguracaoController() { }

        public ConfiguracaoController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

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
        public ActionResult EditarUsuario(EditarUsuarioViewModel model)
        {
            if (ModelState.IsValid)
            {

                User user = (User)UserManager.FindById(User.Identity.GetUserId<int>());

                user.Nome = model.Nome;
                user.Sobrenome = model.Sobrenome;
                user.Email = model.Email;
                user.UserName = model.Email;
                user.DataNascimento = model.DataNascimento.StringToDateTime();
                user.Cor = model.Cor;
                user.PasswordHash = UserManager.PasswordHasher.HashPassword(model.Senha);

                bool saveFailed;
                do
                {
                    saveFailed = false;
                    try
                    {
                        UserManager.Update(user);
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        saveFailed = true;

                        // Update original values from the database 
                        var entry = ex.Entries.Single();
                        entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    }

                } while (saveFailed);
                
                TempData["Alerta"] = "Atualizado com sucesso";
                TempData["Classe"] = "green-alert";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["Alerta"] = "Formulário Inválido";
                TempData["Classe"] = "yellow-alert";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarTelefone([Bind(Prefix = "Item2")] Telefone model)
        {

            if (User.Identity.GetUserId<int>() == model.UsuarioId && ModelState.IsValid)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["Alerta"] = "Atualizado com sucesso";
                TempData["Classe"] = "green-alert";
            }
            else
            {
                TempData["Alerta"] = "Você não tem permissão para realizar essa ação";
                TempData["Classe"] = "yellow-alert";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarCartao([Bind(Prefix = "Item2")] Cartao model)
        {
            if (User.Identity.GetUserId<int>() == model.UsuarioId && ModelState.IsValid)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["Alerta"] = "Atualizado com sucesso";
                TempData["Classe"] = "green-alert";
            }
            else
            {
                TempData["Alerta"] = "Você não tem permissão para realizar essa ação";
                TempData["Classe"] = "yellow-alert";
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirTelefone([Bind(Prefix = "Item2")] Telefone model)
        {
            if (User.Identity.GetUserId<int>() == model.UsuarioId && ModelState.IsValid)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                TempData["Alerta"] = "Removido com sucesso";
                TempData["Classe"] = "green-alert";
            }
            else
            {
                TempData["Alerta"] = "Você não tem permissão para realizar essa ação";
                TempData["Classe"] = "yellow-alert";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirCartao([Bind(Prefix = "Item2")] Cartao model)
        {
            if (User.Identity.GetUserId<int>() == model.UsuarioId && ModelState.IsValid)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                TempData["Alerta"] = "Removido com sucesso";
                TempData["Classe"] = "green-alert";
            }
            else
            {
                TempData["Alerta"] = "Você não tem permissão para realizar essa ação";
                TempData["Classe"] = "yellow-alert";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarTelefone(Telefone model)
        {
            if (ModelState.IsValid)
            {
                model.UsuarioId = User.Identity.GetUserId<int>();
                db.Telefone.Add(model);
                db.SaveChanges();
                TempData["Alerta"] = "Criado com sucesso";
                TempData["Classe"] = "green-alert";
            }
            else
            {
                TempData["Alerta"] = "Formulário inválido";
                TempData["Classe"] = "yellow-alert";
            }

            return RedirectToAction("Index");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarCartao(Cartao model)
        {
            if (ModelState.IsValid)
            {
                model.UsuarioId = User.Identity.GetUserId<int>();
                db.Cartao.Add(model);
                db.SaveChanges();
                TempData["Alerta"] = "Criado com sucesso";
                TempData["Classe"] = "green-alert";
            }
            else
            {
                TempData["Alerta"] = "Formulário inválido";
                TempData["Classe"] = "green-alert";
            }


            return RedirectToAction("Index");
        }
    }
}