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
            model.Telefones = user.Telefones.Select(q => new TelefoneViewModel(q)).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> EditarUsuario(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = model.Update();

                if (user != null)
                {

                    if (user.UserName != model.Email)
                    {
                        if(UserManager.FindByEmail(model.Email) == null)
                        {
                            user.UserName = model.Email;
                            user.Email = model.Email;
                            user.EmailConfirmed = false;
                            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = Util.Encrypt(user.Id.ToString()), code = code }, protocol: Request.Url.Scheme);

                            var mailBody = string.Format(@"
                                <table width=621 border='1' cellpadding='0' cellspacing='0'>

	                                <tr height=160 style='background-color: #106494;'>
		                                <td style='text-align: center; color:white; font-size: 100px;'><span style='font-family: Calibri;'>Task</span><span style='font-family: Impact;'>Quest</span></td>
	                                </tr>

	                                <tr height=300>
		                                <td style='text-align: center;'>
			                                <span style='font-size: 50px; font-family: Impact;'>Confirmação de Cadastro</span>
			                                <br>
			                                <br>
			                                <span style='color: #929496; font-family: Calibri; font-size: 20px;'>Parabéns! Você se registrou no sistema TaskQuest.</span style='font-family:Calibri; font-size: 20px;'><br><br><span style='font-size: 20px;'><a href='{0}' style='text-decoration: none; color: #106494;'>Clique aqui para confirmar o cadastro</a></span>
		                                </td>
	                                </tr>

                                </table>
                            ", callbackUrl);

                            await UserManager.SendEmailAsync(user.Id, "Confirme sua Conta", mailBody);

                            TempData["Alerta"] = "Confirme o email atualizado";
                            TempData["Classe"] = "green-alert";

                            SignOutAsync();

                            return RedirectToAction("ConfirmeEmail", "Account");
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
                        db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        TempData["Alerta"] = "Atualizado com sucesso";
                        TempData["Classe"] = "green-alert";

                        return RedirectToAction("Index");
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

        public ActionResult AlterarSenha()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AlterarSenha(AlterarSenhaViewModel model)
        {
            var user = UserManager.FindById(User.Identity.GetUserId<int>());
            if (UserManager.CheckPassword(user, model.SenhaAtual))
            {
                var result = UserManager.ChangePassword(user.Id, model.SenhaAtual, model.Senha);
                if (result.Succeeded)
                {
                    TempData["Alerta"] = "Senha alterada com sucesso";
                    TempData["Classe"] = "green-alert";
                    return RedirectToAction("Index");
                }

            }

            TempData["Alerta"] = "Algo deu errado";
            TempData["Classe"] = "yellow-alert";
            return RedirectToAction("Index");

        }

        private async void SignOutAsync()
        {
            var clientKey = Request.Browser.Type;
            var user = UserManager.FindById(User.Identity.GetUserId<int>());
            await UserManager.SignOutClientAsync(user, clientKey);
            HttpContext.GetOwinContext().Authentication.SignOut();
            UserManager.UpdateSecurityStamp(User.Identity.GetUserId<int>());
        }

    }
}