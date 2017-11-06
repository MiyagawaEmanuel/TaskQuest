using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using TaskQuest.Models;

namespace TaskQuest.Data.EntityConfiguration
{
    public class PagamentoConfiguration: EntityTypeConfiguration<Pagamento>
    {
        public PagamentoConfiguration()
        {
            ToTable("pag_pagamento");

            HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("pag_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Code)
                .HasColumnName("pag_code")
                .IsRequired();

            Property(e => e.Status)
                .HasColumnName("pag_satus")
                .IsRequired();

            HasRequired(e => e.Grupo)
                .WithOptional(e => e.Pagamento);
        }
    }
}