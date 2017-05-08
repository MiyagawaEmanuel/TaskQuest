using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskQuest.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;

namespace TaskQuest.Identity
{
    // Valida��o do Secutiry Stamp para usu�rio conectado nos clients registrados.
    public static class ApplicationCookieIdentityValidator
    {
        private static async Task<bool> VerifySecurityStampAsync(ApplicationUserManager manager, ApplicationUser user, CookieValidateIdentityContext context)
        {
            string stamp = context.Identity.FindFirstValue("AspNet.Identity.SecurityStamp");
            return (stamp == await manager.GetSecurityStampAsync(context.Identity.GetUserId<int>()));
        }

        private static Task<bool> VerifyClientIdAsync(ApplicationUserManager manager, ApplicationUser user, CookieValidateIdentityContext context)
        {
            string clientId = context.Identity.FindFirstValue("AspNet.Identity.ClientId");
            if (!string.IsNullOrEmpty(clientId) && user.Clients.Any(c => c.Id.ToString() == clientId))
            {
                user.CurrentClientId = clientId;
                return System.Threading.Tasks.Task.FromResult(true);
            }

            return System.Threading.Tasks.Task.FromResult(false);
        }

        public static Func<CookieValidateIdentityContext, System.Threading.Tasks.Task> OnValidateIdentity(TimeSpan validateInterval, Func<ApplicationUserManager, ApplicationUser, Task<ClaimsIdentity>> regenerateIdentity)
        {
            return async context =>
            {
                DateTimeOffset utcNow = context.Options.SystemClock.UtcNow;
                DateTimeOffset? issuedUtc = context.Properties.IssuedUtc;
                bool expired = false;
                if (issuedUtc.HasValue)
                {
                    TimeSpan t = utcNow.Subtract(issuedUtc.Value);
                    expired = (t > validateInterval);
                }
                if (expired)
                {
                    var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
                    int userId = context.Identity.GetUserId<int>();
                    if (userManager != null && userId != 0)
                    {
                        var user = await userManager.FindByIdAsync(userId);
                        bool reject = true;
                        if (user != null
                            && await VerifySecurityStampAsync(userManager, user, context)
                            && await VerifyClientIdAsync(userManager, user, context))
                        {
                            reject = false;
                            if (regenerateIdentity != null)
                            {
                                ClaimsIdentity claimsIdentity = await regenerateIdentity(userManager, user);
                                if (claimsIdentity != null)
                                {
                                    context.OwinContext.Authentication.SignIn(new ClaimsIdentity[]
                                    {
                                        claimsIdentity
                                    });
                                }
                            }
                        }
                        if (reject)
                        {
                            context.RejectIdentity();
                            context.OwinContext.Authentication.SignOut(new string[]
                            {
                                context.Options.AuthenticationType
                            });
                        }
                    }
                }
            };
        }
    }
}