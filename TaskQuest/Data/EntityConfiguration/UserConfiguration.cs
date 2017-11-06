using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TaskQuest.Models;

namespace TaskQuest.Data.EntityConfiguration
{
    public class UserConfiguration: EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("usu_usuario");

            Property(e => e.AccessFailedCount)
                .HasColumnName("usu_contador_acesso_falho");

            Property(e => e.Email)
                .HasColumnName("usu_email")
                .HasMaxLength(40)
                .IsRequired();

            Property(e => e.Email)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new[]
                    {
                        new IndexAttribute("usu_email_unique_idx") { IsUnique = true }
                    }));

            Property(e => e.EmailConfirmed)
                .HasColumnName("usu_email_confirmado");

            HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("usu_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.LockoutEnabled)
                .HasColumnName("usu_bloqueado");

            Property(e => e.LockoutEndDateUtc)
                .HasColumnName("usu_data_desbloqueio");

            Property(e => e.PasswordHash)
                .HasColumnName("usu_senha");

            Ignore(e => e.PhoneNumber);

            Ignore(e => e.PhoneNumberConfirmed);

            Ignore(e => e.CurrentClientId);

            Property(e => e.SecurityStamp)
                .HasColumnName("usu_selo_seguranca");

            Property(e => e.TwoFactorEnabled)
                .HasColumnName("usu_dois_passos_login");

            Property(e => e.UserName)
                .HasColumnName("usu_user_name")
                .IsRequired()
                .HasMaxLength(40);

            Property(e => e.UserName)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new[]
                    {
                        new IndexAttribute("usu_user_name_unique_idx") { IsUnique = true }
                    }));

            Property(e => e.Nome)
                .HasColumnName("usu_nome")
                .HasMaxLength(40)
                .IsRequired();

            Property(e => e.Sobrenome)
                .HasColumnName("usu_sobrenome")
                .IsRequired()
                .HasMaxLength(40);

            Property(e => e.DataNascimento)
                .HasColumnName("usu_data_nascimento");

            Property(e => e.Sexo)
                .HasColumnName("usu_sexo")
                .IsRequired()
                .HasMaxLength(1);

            Property(e => e.Cor)
                .HasColumnName("usu_cor")
                .HasMaxLength(7)
                .IsRequired();

            HasMany(e => e.Clients)
                .WithOptional(e => e.Usuario)
                .HasForeignKey(e => e.UsuarioId)
                .WillCascadeOnDelete(true);

            HasMany(e => e.DestinatarioMensagens)
                .WithOptional(e => e.UsuarioDestinatario)
                .HasForeignKey(e => e.UsuarioDestinatarioId)
                .WillCascadeOnDelete();

            HasMany(e => e.RemetenteMensagens)
                .WithRequired(e => e.UsuarioRemetente)
                .HasForeignKey(e => e.UsuarioRemetenteId);

            HasMany(e => e.Feedbacks)
                .WithOptional(e => e.UsuarioResponsavel)
                .HasForeignKey(e => e.UsuarioResponsavelId)
                .WillCascadeOnDelete(true);

            HasMany(e => e.Quests)
                .WithOptional(e => e.UsuarioCriador)
                .HasForeignKey(e => e.UsuarioCriadorId)
                .WillCascadeOnDelete(true);

            HasMany(e => e.Tasks)
                .WithOptional(e => e.UsuarioResponsavel)
                .HasForeignKey(e => e.UsuarioResponsavelId)
                .WillCascadeOnDelete(true);

            HasMany(e => e.Files)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(true);

            HasMany(e => e.Grupos)
                .WithMany(e => e.Users)
                .Map(m => m.ToTable("uxg_usuario_grupo").MapLeftKey("usu_id").MapRightKey("gru_id"));
        }
    }
}