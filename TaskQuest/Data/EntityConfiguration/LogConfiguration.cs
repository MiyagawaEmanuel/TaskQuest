using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TaskQuest.Models;

namespace TaskQuest.Data.EntityConfiguration
{
    public class LogConfiguration: EntityTypeConfiguration<Log>
    {
        public LogConfiguration()
        {
            ToTable("log_log_transacao");

            HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("log_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.TableName)
                .HasColumnName("log_table_name")
                .IsRequired();

            Property(e => e.QueryType)
                .HasColumnName("log_query_type")
                .IsRequired();

            Property(e => e.Data)
                .HasColumnName("log_data")
                .IsRequired();

        }
    }
}