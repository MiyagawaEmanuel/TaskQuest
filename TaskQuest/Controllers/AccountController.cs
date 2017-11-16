using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TaskQuest.Identity;
using TaskQuest.Models;
using TaskQuest.ViewModels;
using Task = System.Threading.Tasks.Task;
using System.Text.RegularExpressions;
using TaskQuest.Data;

namespace TaskQuest.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        // Definindo a instancia SignInManager presente no request.
        private ApplicationSignInManager _signInManager;

        // Definindo a instancia UserManager presente no request.
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        private async Task SignInAsync(User user, bool isPersistent)
        {
            var clientKey = Request.Browser.Type;
            await UserManager.SignInClientAsync(user, clientKey);
            // Zerando contador de logins errados.
            await UserManager.ResetAccessFailedCountAsync(user.Id);

            // Coletando Claims externos (se houver)
            var ext = await HttpContext.GetOwinContext().Authentication.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);

            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie,
                DefaultAuthenticationTypes.TwoFactorCookie, DefaultAuthenticationTypes.ApplicationCookie);
            HttpContext.GetOwinContext().Authentication.SignIn
            (
                new AuthenticationProperties { IsPersistent = isPersistent },
                // Criação da instancia do Identity e atribuição dos Claims
                await user.GenerateUserIdentityAsync(UserManager, ext)
            );
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl = null)
        {
            if (returnUrl != null)
            {
                TempData["Alerta"] = "Você não está logado";
                TempData["Classe"] = "yellow-alert";
            }
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            var result = await SignInManager.PasswordSignInAsync(model.LoginEmail, model.LoginSenha, true, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    var user = await UserManager.FindAsync(model.LoginEmail, model.LoginSenha);

                    //if (!user.EmailConfirmed)
                    //    return RedirectToAction("ConfirmeEmail", "Account");
                        
                    await SignInAsync(user, true);

                    if (returnUrl != null && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Inicio", "Home");

                case SignInStatus.LockedOut:
                    TempData["Alerta"] = "Você excedeu seu limite de tentativas de entrada";
                    TempData["Classe"] = "yellow-alert";
                    return RedirectToAction("Index", "Home");
                case SignInStatus.Failure:
                default:
                    TempData["Alerta"] = "Email ou Senha incorretos";
                    TempData["Classe"] = "yellow-alert";
                    return RedirectToAction("Index", "Home");
            }
        }

        [AllowAnonymous]
        public ActionResult ConfirmeEmail(string Id)
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ReEnviarEmail()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ReEnviarEmail(string Email)
        {
            var user = UserManager.FindByName(Email);

            if (user != null)
            {
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

                System.Threading.Tasks.Task.Run(() => UserManager.SendEmailAsync(user.Id, "Confirme sua Conta", mailBody));

                TempData["Alerta"] = "Verifique seu email";
                TempData["Classe"] = "green-alert";

                return RedirectToAction("ConfirmeEmail", "Account");
            }
            else
            {
                TempData["Alerta"] = "Email não encontrado";
                TempData["Classe"] = "yellow-alert";
                return View();
            }
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {

            var user = new User()
            {
                Nome = model.Nome,
                Sexo = model.Sexo,
                DataNascimento = model.DataNascimento.StringToDateTime(),
                Sobrenome = model.Sobrenome,
                UserName = model.Email,
                Cor = model.Cor,
                Email = model.Email
            };

            var result = await UserManager.CreateAsync(user, model.Senha);
            if (result.Succeeded)
            {
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

                System.Threading.Tasks.Task.Run(() => UserManager.SendEmailAsync(user.Id, "Confirme sua Conta", mailBody));

                TempData["Alerta"] = "Cadastrado com sucesso";
                TempData["Classe"] = "green-alert";

                return RedirectToAction("ConfirmeEmail", "Account");
            }

            TempData["Alerta"] = "Algo deu errado";
            TempData["Classe"] = "yellow-alert";
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            int Id;
            if (!int.TryParse(Util.Decrypt(userId), out Id) || code == null)
                return View("Error");
            var result = await UserManager.ConfirmEmailAsync(Id, code);
            if (result.Succeeded)
            {
                TempData["Alerta"] = "Email confirmado";
                TempData["Classe"] = "green-alert";
                return RedirectToAction("Login", "Account");
            }
            else
            {
                TempData["Alerta"] = "Algo deu errado";
                TempData["Classe"] = "yellow-alert";
                return RedirectToAction("Index", "Home");
            }
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);

                if (user == null)
                {
                    TempData["Alerta"] = "Algo deu errado";
                    TempData["Classe"] = "yellow-alert";
                    return RedirectToAction("Index", "Home");
                }

                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code },
                    Request.Url.Scheme);
                System.Threading.Tasks.Task.Run(() => UserManager.SendEmailAsync(user.Id, "Esqueci minha senha", string.Format(@"
                    <table width=621 border='1' cellpadding='0' cellspacing='0'>
	                    <tr height=160 style='background-color: #106494;'>
		                    <td style='text-align: center; color:white; font-size: 100px;'><span style='font-family: Calibri;'>Task</span><span style='font-family: Impact;'>Quest</span></td>
	                     </tr>

	                     <tr height=300>
		                    <td style='text-align: center;'>
			                    <span style='font-size: 50px; font-family: Impact;'>Alteração de Senha</span>
			                    <br>
			                    <br>
			                    <span style='color: #929496; font-family: Calibri; font-size: 20px;'>Por favor, altere sua senha </span style='font-family:Calibri; font-size: 20px;'><span style='font-size: 20px;'><a href='{0}' style='text-decoration: none; color: #106494;'>clicando aqui.</a></span>
		                    </td>
	                    </tr>       
                    </table>", callbackUrl)));
                return View("ForgotPasswordConfirmation");
            }

            TempData["Alerta"] = "Algo deu errado";
            TempData["Classe"] = "yellow-alert";
            return View(model);
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(int userID, string code)
        {
            ViewBag.userID = userID;
            ViewBag.code = code;
            return code == null ? View("Error") : View("ResetPassword", new Tuple<string>(code));
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Alerta"] = "Algo deu errado";
                TempData["Classe"] = "yellow-alert";
                return View();
            }

            var user = await UserManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                TempData["Alerta"] = "Algo deu errado";
                TempData["Classe"] = "yellow-alert";
                return View();
            }

            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {

                user.EmailConfirmed = true;
                UserManager.Update(user);

                TempData["Alerta"] = "Senha modificada com sucesso";
                TempData["Classe"] = "green-alert";
                return RedirectToAction("Login", "Account");
            }

            TempData["Alerta"] = "Algo deu errado";
            TempData["Classe"] = "yellow-alert";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user;
                using (var db = new DbContext())
                    user = db.Users.Find(User.Identity.GetUserId<int>());

                if (UserManager.CheckPassword(user, model.Password))
                {
                    UserManager.ChangePassword<User, int>(user.Id, model.Password, model.NewPassword);
                    TempData["Alerta"] = "Senha alterada com sucesso";
                    TempData["Classe"] = "green-alert";
                    return RedirectToAction("Index", "Configuracao");
                }
                else
                {
                    TempData["Alerta"] = "Senha incorreta";
                    TempData["Classe"] = "yellow-alert";
                }
            }
            else
            {
                TempData["Alerta"] = "Algo deu errado";
                TempData["Classe"] = "yellow-alert";
            }
            return View();
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogOff()
        {
            await SignOutAsync();
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignOutAsync()
        {
            var clientKey = Request.Browser.Type;
            var user = UserManager.FindById(User.Identity.GetUserId<int>());
            await UserManager.SignOutClientAsync(user, clientKey);
            HttpContext.GetOwinContext().Authentication.SignOut();
        }

        public async Task<ActionResult> SignOutEverywhere()
        {
            UserManager.UpdateSecurityStamp(User.Identity.GetUserId<int>());
            await SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignOutClient(int clientId)
        {
            var user = UserManager.FindById(User.Identity.GetUserId<int>());
            var client = user.Clients.SingleOrDefault(c => c.Id == clientId);
            if (client != null)
                user.Clients.Remove(client);
            UserManager.Update(user);
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                    properties.Dictionary[XsrfKey] = UserId;
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
    }
}
