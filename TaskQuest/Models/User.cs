using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TaskQuest.Models
{
    public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
        public User()
        {
            Clients = new HashSet<Client>();
            Cartoes = new HashSet<Cartao>();
            RemetenteMensagens = new HashSet<Mensagem>();
            DestinatarioMensagens = new HashSet<Mensagem>();
            Feedbacks = new HashSet<Feedback>();
            Tasks = new HashSet<Task>();
            Quests = new HashSet<Quest>();
            Telefones = new HashSet<Telefone>();
            Grupos = new HashSet<Grupo>();
            ExperienciaUsuarios = new HashSet<ExperienciaUsuario>();
        }

        public string Nome { get; set; }

        public string Sobrenome { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Sexo { get; set; }

        public string Cor { get; set; }

        public virtual ICollection<Cartao> Cartoes { get; set; }

        public virtual ICollection<Mensagem> RemetenteMensagens { get; set; }

        public virtual ICollection<Mensagem> DestinatarioMensagens { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; }

        public virtual ICollection<Quest> Quests { get; set; }

        public virtual ICollection<Telefone> Telefones { get; set; }

        public virtual ICollection<Grupo> Grupos { get; set; }

        public virtual ICollection<ExperienciaUsuario> ExperienciaUsuarios { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
        
        public string CurrentClientId { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager,
            ClaimsIdentity ext = null)
        {
            // Observe que o authenticationType precisa ser o mesmo que foi definido em CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(CurrentClientId))
                claims.Add(new Claim("AspNet.Identity.ClientId", CurrentClientId));

            //  Adicione novos Claims aqui //

            // Adicionando Claims externos capturados no login
            if (ext != null)
                SetExternalProperties(userIdentity, ext);

            // Gerenciamento de Claims para informacoes do usuario
            //claims.Add(new Claim("AdmRoles", "True"));

            userIdentity.AddClaims(claims);

            return userIdentity;
        }

        private void SetExternalProperties(ClaimsIdentity identity, ClaimsIdentity ext)
        {
            if (ext != null)
            {
                var ignoreClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims";
                // Adicionando Claims Externos no Identity
                foreach (var c in ext.Claims)
                    if (!c.Type.StartsWith(ignoreClaim))
                        if (!identity.HasClaim(c.Type, c.Value))
                            identity.AddClaim(c);
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager)
        {
            // Note the authenticationType must match the one defined in
            // CookieAuthenticationOptions.AuthenticationType 
            var userIdentity = await manager.CreateIdentityAsync(
                this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here 
            return userIdentity;
        }
    }
}