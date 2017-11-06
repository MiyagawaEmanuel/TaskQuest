using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TaskQuest.Models;

namespace TaskQuest.Data.EntityConfiguration
{
    public class TelefoneConfiguration: EntityTypeConfiguration<Telefone>
    {
        public TelefoneConfiguration()
        {
            ToTable("tel_telefone");

            HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("tel_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.UsuarioId)
                .HasColumnName("usu_id")
                .IsRequired();

            Property(e => e.Numero)
                .HasColumnName("tel_numero")
                .IsRequired();

            Property(e => e.Tipo)
                .HasColumnName("tel_tipo")
                .HasMaxLength(40)
                .IsRequired();
        }
    }
}