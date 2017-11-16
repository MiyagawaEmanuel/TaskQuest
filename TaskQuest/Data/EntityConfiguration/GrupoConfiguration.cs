using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TaskQuest.Models;

namespace TaskQuest.Data.EntityConfiguration
{
    public class GrupoConfiguration: EntityTypeConfiguration<Grupo>
    {
        public GrupoConfiguration()
        {
            ToTable("gru_grupo");

            HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("gru_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Nome)
                .HasColumnName("gru_nome")
                .HasMaxLength(40)
                .IsRequired();

            Property(e => e.Cor)
                .HasColumnName("gru_cor")
                .HasMaxLength(7)
                .IsRequired();

            Property(e => e.DataCriacao)
                .HasColumnName("gru_data_criacao")
                .IsRequired();

            Property(e => e.Descricao)
                .HasColumnName("gru_descricao")
                .HasMaxLength(120)
                .IsRequired();

            HasMany(e => e.Quests)
                .WithOptional(e => e.GrupoCriador)
                .HasForeignKey(e => e.GrupoCriadorId)
                .WillCascadeOnDelete();

            HasMany(e => e.Mensagens)
                .WithOptional(e => e.GrupoDestinatario)
                .HasForeignKey(e => e.GrupoDestinatarioId)
                .WillCascadeOnDelete();

            HasMany(e => e.Notificacoes)
                .WithRequired(e => e.Grupo)
                .HasForeignKey(e => e.GrupoId)
                .WillCascadeOnDelete();

            HasOptional(e => e.Pagamento)
                .WithRequired(e => e.Grupo);

        }
    }
}