using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TaskQuest.Models;

namespace TaskQuest.Data.EntityConfiguration
{
    public class FileConfiguration: EntityTypeConfiguration<File>
    {
        public FileConfiguration()
        {
            ToTable("fle_file");

            HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("fle_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Nome)
                .HasColumnName("fle_nome")
                .HasMaxLength(120)
                .IsRequired();

            Property(e => e.Url)
                .HasColumnName("fle_url")
                .HasMaxLength(40)
                .IsRequired();

            Property(e => e.Response)
                .HasColumnName("fle_response")
                .HasMaxLength(5)
                .IsRequired();

            Property(e => e.Size)
                .HasColumnName("fle_size")
                .IsRequired();

            Property(e => e.DataEnvio)
                .HasColumnName("fle_data_envio")
                .IsRequired();

            Property(e => e.TaskId)
                .HasColumnName("tsk_id");

            Property(e => e.IsValid)
                .HasColumnName("fle_validado")
                .IsRequired();

            Property(e => e.UserId)
                .HasColumnName("usu_id")
                .IsRequired();
        }
    }
}