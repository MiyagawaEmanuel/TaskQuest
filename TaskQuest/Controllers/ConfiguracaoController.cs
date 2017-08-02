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
            get 
            { 
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set 
            { 
                _userManager = value; 
            }
        }

        private DbContext db = new DbContext();

        public ActionResult Index()
        {
            var model = new ConfiguracaoViewModel();

            var user = db.Users.Find(User.Identity.GetUserId<int>());

            model.usuario = new UserViewModel(user);
            model.Cartoes = user.Cartoes.Select(q => new CartaoViewModel(q)).ToList();
            model.Telefones = user.Telefones.Select(q => new TelefoneViewModel(q)).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarUsuario(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = model.Update();

                if (user != null)
                {
                    user.PasswordHash = UserManager.PasswordHasher.HashPassword(user.PasswordHash);

                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    TempData["Alerta"] = "Atualizado com sucesso";
                    TempData["Classe"] = "green-alert";

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Alerta"] = "Algo deu errado";
                    TempData["Classe"] = "yellow-alert";
                    return RedirectToAction("Index");
                }
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
        public ActionResult EditarTelefone([Bind(Prefix = "Item2")] TelefoneViewModel model)
        {
            if (ModelState.IsValid)
            {
                var telefone = model.Update();
                if (telefone != null)
                {
                    if (User.Identity.GetUserId<int>() == telefone.UsuarioId)
                    {
                        db.Entry(telefone).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        TempData["Alerta"] = "Atualizado com sucesso";
                        TempData["Classe"] = "green-alert";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Alerta"] = "Você não pode executar esta ação";
                        TempData["Classe"] = "yellow-alert";
                        return RedirectToAction("Inicio", "Home");
                    }
                }
                else
                {
                    TempData["Alerta"] = "Algo deu errado";
                    TempData["Classe"] = "yellow-alert";
                    return RedirectToAction("Index");
                }
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
        public ActionResult EditarCartao([Bind(Prefix = "Item2")] CartaoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var cartao = model.Update();
                if (cartao != null)
                {
                    if (User.Identity.GetUserId<int>() == cartao.UsuarioId)
                    {
                        db.Entry(cartao).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        TempData["Alerta"] = "Atualizado com sucesso";
                        TempData["Classe"] = "green-alert";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Alerta"] = "Você não pode executar esta ação";
                        TempData["Classe"] = "yellow-alert";
                        return RedirectToAction("Inicio", "Home");
                    }
                }
                else
                {
                    TempData["Alerta"] = "Algo deu errado";
                    TempData["Classe"] = "yellow-alert";
                    return RedirectToAction("Index");
                }
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
        public ActionResult ExcluirTelefone([Bind(Prefix = "Item2")] TelefoneViewModel model)
        {
            if (ModelState.IsValid)
            {
                var telefone = model.Update();
                if (telefone != null)
                {
                    if (User.Identity.GetUserId<int>() == telefone.UsuarioId)
                    {
                        db.Entry(telefone).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                        TempData["Alerta"] = "Deletado com sucesso";
                        TempData["Classe"] = "green-alert";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Alerta"] = "Você não pode executar esta ação";
                        TempData["Classe"] = "yellow-alert";
                        return RedirectToAction("Inicio", "Home");
                    }
                }
                else
                {
                    TempData["Alerta"] = "Algo deu errado";
                    TempData["Classe"] = "yellow-alert";
                    return RedirectToAction("Index");
                }
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
        public ActionResult ExcluirCartao([Bind(Prefix = "Item2")] CartaoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var cartao = model.Update();
                if (cartao != null)
                {
                    if (User.Identity.GetUserId<int>() == cartao.UsuarioId)
                    {
                        db.Entry(cartao).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                        TempData["Alerta"] = "Deletado com sucesso com sucesso";
                        TempData["Classe"] = "green-alert";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Alerta"] = "Você não pode executar esta ação";
                        TempData["Classe"] = "yellow-alert";
                        return RedirectToAction("Inicio", "Home");
                    }
                }
                else
                {
                    TempData["Alerta"] = "Algo deu errado";
                    TempData["Classe"] = "yellow-alert";
                    return RedirectToAction("Index");
                }
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
        public ActionResult AdicionarTelefone(TelefoneViewModel model)
        {
            if (ModelState.IsValid)
            {
                var telefone = model.Update();
                if (telefone != null)
                {
                    telefone.UsuarioId = User.Identity.GetUserId<int>();
                    db.Telefone.Add(telefone);
                    db.SaveChanges();
                    TempData["Alerta"] = "Criado com sucesso";
                    TempData["Classe"] = "green-alert";
                }
                else
                {
                    TempData["Alerta"] = "Algo deu errado";
                    TempData["Classe"] = "yellow-alert";
                }
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
        public ActionResult AdicionarCartao(CartaoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var cartao = model.Update();
                if (cartao != null)
                {
                    cartao.UsuarioId = User.Identity.GetUserId<int>();
                    db.Cartao.Add(cartao);
                    db.SaveChanges();
                    TempData["Alerta"] = "Criado com sucesso";
                    TempData["Classe"] = "green-alert";
                }
                else
                {
                    TempData["Alerta"] = "Algo deu errado";
                    TempData["Classe"] = "yellow-alert";
                }
            }
            else
            {
                TempData["Alerta"] = "Formulário inválido";
                TempData["Classe"] = "yellow-alert";
            }


            return RedirectToAction("Index");
        }
    }
}