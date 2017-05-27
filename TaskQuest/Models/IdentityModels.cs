using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TaskQuest.Models
{
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
        public CustomRole()
        {
        }

        public CustomRole(string name)
        {
            Name = name;
        }

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