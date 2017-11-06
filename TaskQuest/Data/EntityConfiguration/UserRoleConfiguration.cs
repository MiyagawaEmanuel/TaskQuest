using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TaskQuest.Models;

namespace TaskQuest.Data.EntityConfiguration
{
    public class UserRoleConfiguration : EntityTypeConfiguration<UserRole>
    {
        public UserRoleConfiguration() 
        {
            ToTable("cur_custom_user_role");

            HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("cur_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.UserId)
                .HasColumnName("usu_id");

            Property(e => e.RoleId)
                .HasColumnName("rol_id");
        }
    }
}