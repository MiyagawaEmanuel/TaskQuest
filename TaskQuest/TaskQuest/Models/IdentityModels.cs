using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TaskQuest.Models
{
    [Table("usu_usuario")]
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationUser()
        {
            Clients = new Collection<Client>();
            crt_cartao = new HashSet<crt_cartao>();
            msg_mensagem = new HashSet<msg_mensagem>();
            msg_mensagem1 = new HashSet<msg_mensagem>();
            pre_precedencia = new HashSet<pre_precedencia>();
            qst_quest = new HashSet<qst_quest>();
            tel_telefone = new HashSet<tel_telefone>();
            uxg_usuario_grupo = new HashSet<uxg_usuario_grupo>();
            xpu_experiencia_usuario = new HashSet<xpu_experiencia_usuario>();
        }

        [Required]
        [StringLength(20)]
        public string usu_nome { get; set; }

        [Required]
        [StringLength(20)]
        public string usu_sobrenome { get; set; }

        [Column(TypeName = "date")]
        public DateTime usu_data_nascimento { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(1)]
        public string usu_sexo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<crt_cartao> crt_cartao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<msg_mensagem> msg_mensagem { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<msg_mensagem> msg_mensagem1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pre_precedencia> pre_precedencia { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<qst_quest> qst_quest { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tel_telefone> tel_telefone { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<uxg_usuario_grupo> uxg_usuario_grupo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<xpu_experiencia_usuario> xpu_experiencia_usuario { get; set; }

        public virtual ICollection<Client> Clients { get; set; }

        [NotMapped]
        public string CurrentClientId { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager, ClaimsIdentity ext = null)
        {
            // Observe que o authenticationType precisa ser o mesmo que foi definido em CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(CurrentClientId))
            {
                claims.Add(new Claim("AspNet.Identity.ClientId", CurrentClientId));
            }

            //  Adicione novos Claims aqui //

            // Adicionando Claims externos capturados no login
            if (ext != null)
            {
                SetExternalProperties(userIdentity, ext);
            }

            // Gerenciamento de Claims para informaçoes do usuario
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
                {
                    if (!c.Type.StartsWith(ignoreClaim))
                        if (!identity.HasClaim(c.Type, c.Value))
                            identity.AddClaim(c);
                }
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

    public class CustomUserRole : IdentityUserRole<int>
    {
        [Key]
        public int CustomUserRoleId { get; set; }
    }

    public class CustomUserClaim : IdentityUserClaim<int>
    {
        [Key]
        public int CustomUserClaimId { get; set; }
    }

    public class CustomUserLogin : IdentityUserLogin<int>
    {
        [Key]
        public int CustomUserLoginId { get; set; }
    }

    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }
        
        [Key]
        public int CustomRoleId { get; set; }

    }

    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, int,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(DbContext context)
            : base(context)
        {
        }

        [Key]
        public int CustomUserStoreId { get; set; }

    }

    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
    {
        public CustomRoleStore(DbContext context)
            : base(context)
        {
        }

        [Key]
        public int CustomRoleStoreId { get; set; }

    }

}
