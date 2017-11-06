using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TaskQuest.Models;

namespace TaskQuest.Data.EntityConfiguration
{
    public class TaskConfiguration: EntityTypeConfiguration<Task>
    {
        public TaskConfiguration()
        {
            ToTable("tsk_task");

            HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("tsk_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.QuestId)
                .HasColumnName("qst_id")
                .IsRequired();

            Property(e => e.Nome)
                .HasColumnName("tsk_nome")
                .HasMaxLength(40)
                .IsRequired();

            Property(e => e.Descricao)
                .HasColumnName("tsk_descricao")
                .HasMaxLength(300)
                .IsRequired();

            Property(e => e.Status)
                .HasColumnName("tsk_status")
                .IsRequired();

            Property(e => e.Dificuldade)
                .HasColumnName("tsk_dificuldade")
                .IsRequired();

            Property(e => e.DataCriacao)
                .HasColumnName("tsk_data_criacao")
                .IsRequired();

            Property(e => e.DataConclusao)
                .HasColumnName("tsk_data_conclusao")
                .IsRequired();

            Property(e => e.RequerVerificacao)
                .HasColumnName("tsk_verificacao")
                .IsRequired();

            Property(e => e.UsuarioResponsavelId)
                .HasColumnName("usu_id_responsavel");

            Property(e => e.DataInicio)
                .HasColumnName("tsk_data_inicio");

            HasMany(e => e.Files)
                .WithOptional(e => e.Task)
                .HasForeignKey(e => e.TaskId)
                .WillCascadeOnDelete();
        }
    }
}