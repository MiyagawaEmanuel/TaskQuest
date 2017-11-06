using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TaskQuest.Models;

namespace TaskQuest.Data.EntityConfiguration
{
    public class PontoUsuarioConfiguration: EntityTypeConfiguration<PontoUsuario>
    {
        public PontoUsuarioConfiguration()
        {

            HasKey(e => new { e.TaskId, e.UsuarioId });

            ToTable("ptu_ponto_usuario");

            Property(e => e.TaskId)
                .HasColumnName("ptu_id");

            Property(e => e.UsuarioId)
                .HasColumnName("usu_id");

            Property(e => e.Valor)
                .HasColumnName("ptu_valor")
                .IsRequired();

            Property(e => e.Data)
                .HasColumnName("ptu_data")
                .IsRequired();
        }
    }
}