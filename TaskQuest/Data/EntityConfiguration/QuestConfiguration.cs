using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TaskQuest.Models;

namespace TaskQuest.Data.EntityConfiguration
{
    public class QuestConfiguration: EntityTypeConfiguration<Quest>
    {
        public QuestConfiguration()
        {
            ToTable("qst_quest");

            HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("qst_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.UsuarioCriadorId)
                .HasColumnName("usu_id_criador");

            Property(e => e.GrupoCriadorId)
                .HasColumnName("gru_id_criador");

            Property(e => e.Cor)
                .HasColumnName("qst_cor")
                .HasMaxLength(7)
                .IsRequired();

            Property(e => e.DataCriacao)
                .HasColumnName("qst_data_criacao")
                .IsRequired();

            Property(e => e.Descricao)
                .HasColumnName("qst_descricao")
                .HasMaxLength(300)
                .IsRequired();

            Property(e => e.Nome)
                .HasColumnName("qst_nome")
                .HasMaxLength(40)
                .IsRequired();
        }
    }
}