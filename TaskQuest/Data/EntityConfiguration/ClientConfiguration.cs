using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TaskQuest.Models;

namespace TaskQuest.Data.EntityConfiguration
{
    public class ClientConfiguration: EntityTypeConfiguration<Client>
    {

        public ClientConfiguration()
        {
            ToTable("cli_client");

            HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("cli_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.ClientKey)
                .HasColumnName("cli_key");

            Property(e => e.UsuarioId)
                .HasColumnName("usu_id");
        }

    }
}