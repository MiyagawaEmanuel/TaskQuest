using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TaskQuest.Models;

namespace TaskQuest.Data.EntityConfiguration
{
    public class FeedbackConfiguration: EntityTypeConfiguration<Feedback>
    {
        public FeedbackConfiguration()
        {
            ToTable("feb_feedback");

            HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("feb_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Relatorio)
                .HasColumnName("feb_relatorio")
                .HasMaxLength(300);

            Property(e => e.Resposta)
                .HasColumnName("feb_resposta")
                .HasMaxLength(300);

            Property(e => e.Nota)
                .HasColumnName("feb_nota")
                .IsRequired();

            Property(e => e.DataCriacao)
                .HasColumnName("feb_data_criacao")
                .IsRequired();

            Property(e => e.TaskId)
                .HasColumnName("tsk_id")
                .IsRequired();

            Property(e => e.UsuarioResponsavelId)
                .HasColumnName("usu_id_responsavel");
        }
    }
}