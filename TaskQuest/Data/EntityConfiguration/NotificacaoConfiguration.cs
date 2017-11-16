using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TaskQuest.Models;

namespace TaskQuest.Data.EntityConfiguration
{
    public class NotificacaoConfiguration: EntityTypeConfiguration<Notificacao>
    {
        public NotificacaoConfiguration()
        {
            ToTable("not_notificacao");

            HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("not_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Texto)
                .HasColumnName("not_texto")
                .IsRequired();

            Property(e => e.TipoNotificacao)
                .HasColumnName("not_tipo_notificacao")
                .IsRequired();

            Property(e => e.EntidadeModificada)
                .HasColumnName("not_entidade_modificada")
                .IsRequired();

            Property(e => e.DataNotificacao)
                .HasColumnName("not_data_notificacao")
                .IsRequired();

            Property(e => e.GrupoId)
                .HasColumnName("gru_id")
                .IsRequired();
        }
    }
}