using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TaskQuest.Models;

namespace TaskQuest.Data.EntityConfiguration
{
    public class UserLoginConfiguration: EntityTypeConfiguration<UserLogin>
    {
        public UserLoginConfiguration()
        {
            ToTable("cul_custom_user_login");

            HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("cul_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.UserId)
                .HasColumnName("usu_id");

            Property(e => e.LoginProvider)
                .HasColumnName("cul_login_provider");

            Property(e => e.ProviderKey)
                .HasColumnName("cul_provider_key");
        }
    }
}