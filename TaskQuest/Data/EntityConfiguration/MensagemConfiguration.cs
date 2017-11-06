using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TaskQuest.Models;

namespace TaskQuest.Data.EntityConfiguration
{
    public class MensagemConfiguration: EntityTypeConfiguration<Mensagem>
    {
        public MensagemConfiguration()
        {
            ToTable("msg_mensagem");

            HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("msg_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.UsuarioRemetenteId)
                .HasColumnName("usu_id_remetente")
                .IsRequired();

            Property(e => e.UsuarioDestinatarioId)
                .HasColumnName("usu_id_destinatario");

            Property(e => e.GrupoDestinatarioId)
                .HasColumnName("gru_id_destinatario");

            Property(e => e.Conteudo)
                .HasColumnName("msg_conteudo")
                .HasMaxLength(120).IsRequired();

            
            Property(e => e.Data)
                .HasColumnName("msg_data")
                .IsRequired();
        }
    }
}