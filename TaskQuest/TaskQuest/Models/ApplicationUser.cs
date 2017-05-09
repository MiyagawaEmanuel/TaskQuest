using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TaskQuest.Models
{
    [Table("usu_usuario")]
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationUser()
        {
            Clients = new Collection<Client>();
            Cartoes = new HashSet<Cartao>();
            RemetenteMensagens = new HashSet<Mensagem>();
            DestinatarioMensagens = new HashSet<Mensagem>();
            Feedbacks = new HashSet<Feedback>();
            Tasks = new HashSet<Task>();
            Quests = new HashSet<Quest>();
            Telefones = new HashSet<Telefone>();
            UsuarioGrupos = new HashSet<UsuarioGrupo>();
            ExperienciaUsuarios = new HashSet<ExperienciaUsuario>();
        }

        [Required]
        [StringLength(20)]
        [Column("usu_sobrenome")]
        public string Sobrenome { get; set; }

        [Column("usu_data_nascimento", TypeName = "date")]
        public DateTime DataNascimento { get; set; }

        [Column("usu_sexo", TypeName = "char")]
        [Required]
        [StringLength(1)]
        public string Sexo { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cartao> Cartoes { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mensagem> RemetenteMensagens { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mensagem> DestinatarioMensagens { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task> Tasks { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feedback> Feedbacks { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Quest> Quests { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Telefone> Telefones { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsuarioGrupo> UsuarioGrupos { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExperienciaUsuario> ExperienciaUsuarios { get; set; }

        public virtual ICollection<Client> Clients { get; set; }

        [NotMapped]
        public string CurrentClientId { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager,
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

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
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