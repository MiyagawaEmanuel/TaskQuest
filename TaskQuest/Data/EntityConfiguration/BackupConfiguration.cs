using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TaskQuest.Models;

namespace TaskQuest.Data.EntityConfiguration
{
    public class BackupConfiguration: EntityTypeConfiguration<Backup>
    {
        public BackupConfiguration()
        {
            ToTable("bkp_backup");

            HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("bkp_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.TableName)
                .HasColumnName("bkp_table_name")
                .IsRequired();

            Property(e => e.QueryType)
                .HasColumnName("bkp_query_type")
                .IsRequired();

            Property(e => e.Data)
                .HasColumnName("bkp_data")
                .IsRequired();

        }
    }
}