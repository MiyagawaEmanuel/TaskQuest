using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using TaskQuest.Models;
using TaskQuest.Data;

namespace TaskQuest.Identity
{
    // Configuração do UserManager Customizado

    public class ApplicationUserManager : UserManager<User, int>
    {
        public ApplicationUserManager(IUserStore<User, int> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore(context.Get<DbContext>()));

            // Configurando validator para nome de usuario
            manager.UserValidator = new UserValidator<User, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Logica de validação e complexidade de senha
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };
            // Configuração de Lockout
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(2);
            manager.MaxFailedAccessAttemptsBeforeLockout = 3;

            // Definindo a classe de serviço de e-mail
            manager.EmailService = new EmailService();

            var dataProtectionProvider = options.DataProtectionProvider;

            if (dataProtectionProvider != null)
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<User, int>(
                        dataProtectionProvider.Create("TaskQuest Token"));

            return manager;
        }

        // Metodo para login async que guarda os dados Client conectado
        public async Task<IdentityResult> SignInClientAsync(User user, string clientKey)
        {
            if (string.IsNullOrEmpty(clientKey))
                throw new ArgumentNullException("clientKey");

            var client = user.Clients.SingleOrDefault(c => c.ClientKey == clientKey);
            if (client == null)
            {
                client = new Client {ClientKey = clientKey};
                user.Clients.Add(client);
            }

            var result = await UpdateAsync(user);
            user.CurrentClientId = client.Id.ToString();
            return result;
        }

        // Metodo para login async que remove os dados Client conectado
        public async Task<IdentityResult> SignOutClientAsync(User user, string clientKey)
        {
            if (string.IsNullOrEmpty(clientKey))
                throw new ArgumentNullException("clientKey");

            var client = user.Clients.SingleOrDefault(c => c.ClientKey == clientKey);
            if (client != null)
                user.Clients.Remove(client);

            user.CurrentClientId = null;
            return await UpdateAsync(user);
        }
    }
}