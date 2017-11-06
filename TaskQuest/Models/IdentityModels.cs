using Microsoft.AspNet.Identity.EntityFramework;
using TaskQuest.Data;

namespace TaskQuest.Models
{
    public class UserRole : IdentityUserRole<int>
    {
        public int Id { get; set; }
    }

    public class UserClaim : IdentityUserClaim<int>
    {
        public UserClaim() { }

        public UserClaim(string type, string value)
        {
            this.ClaimType = type;
            this.ClaimValue = value;
        }
    }

    public class UserLogin : IdentityUserLogin<int>
    {
        public int Id { get; set; }
    }

    public class Role : IdentityRole<int, UserRole>
    {
        public Role()
        {
        }

        public Role(string name)
        {
            Name = name;
        }
    }

    public class UserStore : UserStore<User, Role, int,
        UserLogin, UserRole, UserClaim>
    {
        public UserStore(DbContext context)
            : base(context)
        {
        }

        public int Id { get; set; }
    }

    public class RoleStore : RoleStore<Role, int, UserRole>
    {
        public RoleStore(DbContext context)
            : base(context)
        {
        }

        public int Id { get; set; }
    }
}