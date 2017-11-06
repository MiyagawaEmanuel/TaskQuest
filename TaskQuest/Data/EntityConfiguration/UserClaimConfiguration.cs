using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TaskQuest.Models;

namespace TaskQuest.Data.EntityConfiguration
{
    public class UserClaimConfiguration: EntityTypeConfiguration<UserClaim>
    {
        public UserClaimConfiguration()
        {
            ToTable("cuc_custom_user_claim");

            HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("cuc_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.UserId)
                .HasColumnName("usu_id");

            Property(e => e.ClaimType)
                .HasColumnName("cuc_type");

            Property(e => e.ClaimValue)
                .HasColumnName("cuc_value");
        }
    }
}