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
using TaskQuest.App_Code;
using Task = System.Threading.Tasks.Task;

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
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            private set => _signInManager = value;
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private async Task SignInAsync(User user, bool isPersistent)
        {
            var clientKey = Request.Browser.Type;
            await UserManager.SignInClientAsync(user, clientKey);
            // Zerando contador de logins errados.
            await UserManager.ResetAccessFailedCountAsync(user.Id);

            // Coletando Claims externos (se houver)
            var ext = await AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie,
                DefaultAuthenticationTypes.TwoFactorCookie, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn
            (
                new AuthenticationProperties { IsPersistent = isPersistent },
                // Criação da instancia do Identity e atribuição dos Claims
                await user.GenerateUserIdentityAsync(UserManager, ext)
            );
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            var result = await SignInManager.PasswordSignInAsync(model.LoginEmail, model.LoginSenha, true, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    var user = await UserManager.FindAsync(model.LoginEmail, model.LoginSenha);
                    await SignInAsync(user, true);
                    return RedirectToAction("Inicio", "Home");
                case SignInStatus.LockedOut:
                    TempData["Response"] = "Você excedeu seu limite de tentativas de entrada";
                    TempData["Class"] = "yellow-alert";
                    return RedirectToAction("Index", "Home");
                case SignInStatus.Failure:
                default:
                    TempData["Response"] = "Email ou Senha incorretos";
                    TempData["Class"] = "yellow-alert";
                    return RedirectToAction("Index", "Home");
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
                Cor = model.Cor.HexToColor(),
                Email = model.Email
            };

            if(user.Cor == null)
            {
                TempData["Response"] = "Cor inválida";
                TempData["Class"] = "yellow-alert";
                return RedirectToAction("Inicio", "Home");
            }

            var result = await UserManager.CreateAsync(user, model.Senha);
            if (result.Succeeded)
            {
                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                await UserManager.ConfirmEmailAsync(user.Id, code);
                
                return RedirectToAction("Index", "Home");
            }
            
            AddErrors(result);

            // No caso de falha, reexibir a view. 
            TempData["Response"] = "Algo deu errado";
            TempData["Class"] = "yellow-alert";
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            if (userId == 0 || code == null)
                return View("Error");
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "Inicio" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !await UserManager.IsEmailConfirmedAsync(user.Id))
                    return View("ForgotPasswordConfirmation");

                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code },
                    Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Esqueci minha senha",
                    "Por favor altere sua senha clicando aqui: <a href='" + callbackUrl + "'></a>");
                return View("ForgotPasswordConfirmation");
            }

            // No caso de falha, reexibir a view. 
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/FacebookLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult FacebookLogin()
        {
            return new ChallengeResult("Facebook", "/Account/ExternalLoginCallback");
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
                return RedirectToAction("Login");

            var externalIdentity = HttpContext.GetOwinContext().Authentication
                .GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
            var email = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "urn:facebook:email").Value;
            var firstName = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "urn:facebook:first_name")
                .Value;
            var lastName = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "urn:facebook:last_name").Value;
            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
            return View("PickColor");
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PickColor(string color)
        {
            await UserManager.AddClaimAsync(User.Identity.GetUserId<int>(), new Claim("Color", color));
            return View("Inicio");
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogOff()
        {
            await SignOutAsync();
            //AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);
        }

        private async Task SignOutAsync()
        {
            var clientKey = Request.Browser.Type;
            var user = UserManager.FindById(User.Identity.GetUserId<int>());
            await UserManager.SignOutClientAsync(user, clientKey);
            AuthenticationManager.SignOut();
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

        [Route("EmailDisponivel?Email={email}")]
        public JsonResult EmailDisponivel(string email)
        {
            try
            {
                User user = UserManager.FindByEmail(email);
                return Json(new { response = false });
            }
            catch
            {
                return Json(new { response = true });
            }
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