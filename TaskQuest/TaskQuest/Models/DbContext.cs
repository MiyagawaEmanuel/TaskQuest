using System.Data.Entity;
using System;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.Entity;

namespace TaskQuest.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class DbContext : IdentityDbContext<ApplicationUser, CustomRole, int,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public DbContext()
            : base("DefaultConnection")
        {
            DbConfiguration.SetConfiguration(new MySqlEFConfiguration());
            Database.SetInitializer(new DropCreateDatabaseAlways<DbContext>());
        }

        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Cartao> Cartoes { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Grupo> Grupos { get; set; }
        public virtual DbSet<Mensagem> Mensagens { get; set; }
        public virtual DbSet<Precedencia> Precedencia { get; set; }
        public virtual DbSet<Quest> Quest { get; set; }
        public virtual DbSet<Semana> Semana { get; set; }
        public virtual DbSet<Telefone> Telefone { get; set; }
        public virtual DbSet<Arquivo> Arquivo { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<UsuarioGrupo> UsuarioGrupo { get; set; }
        public virtual DbSet<ExperienciaGrupo> ExperienciaGrupo { get; set; }
        public virtual DbSet<ExperienciaUsuario> ExperienciaUsuario { get; set; }
        public virtual DbSet<Claims> Claims { get; set; }

        public static DbContext Create()
        {
            return new DbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            //Remove uma configuração embutida no Entity que modifica os nomes das tabelas e campos
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            
            /*
                Configurando tabela ApplicationUser (usu_usuario)
            */
            
            modelBuilder.Entity<ApplicationUser>()
                .ToTable("usu_usuario");
            
            modelBuilder.Entity<ApplicationUser>() 
                .Property(e => e.AccessFailedCount) 
                .HasColumnName("usu_contador_acesso_falho");
                
            modelBuilder.Entity<ApplicationUser>() 
                .Property(e => e.Claims) 
                .HasColumnName("usu_claims");
                
            modelBuilder.Entity<ApplicationUser>() 
                .Property(e => e.Email) 
                .HasColumnName("usu_email");
            
            modelBuilder.Entity<ApplicationUser>().Property(t => t.Email).IsRequired();
            
            modelBuilder.Entity<ApplicationUser>().Property(t => t.Email).HasMaxLength(50);
            
            modelBuilder.Entity<ApplicationUser>() 
                .Property(e => e.Email) 
                .HasColumnAnnotation( 
                    "Index",  
                    new IndexAnnotation(new[] 
                    { 
                        new IndexAttribute("email_unique_idx") { IsUnique = true } 
                    })));
            
            //Configurando coluna como Not Null
            modelBuilder.Entity<ApplicationUser>().Property(t => t.Email).IsRequired();
                
            modelBuilder.Entity<ApplicationUser>() 
                .Property(e => e.EmailConfirmed) 
                .HasColumnName("usu_email_confirmado");
                
            modelBuilder.Entity<ApplicationUser>() 
                .Property(e => e.Id) 
                .HasColumnName("usu_id");
                
            modelBuilder.Entity<ApplicationUser>().Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOprion.Identity);
                
            modelBuilder.Entity<ApplicationUser>() 
                .Property(e => e.LockoutEnabled) 
                .HasColumnName("usu_bloqueado");
                
            modelBuilder.Entity<ApplicationUser>() 
                .Property(e => e.LockoutEndDateUtc) 
                .HasColumnName("usu_data_desbloqueio");
                
            modelBuilder.Entity<ApplicationUser>() 
                .Property(e => e.Logins) 
                .HasColumnName("usu_logins");
                
            modelBuilder.Entity<ApplicationUser>() 
                .Property(e => e.PasswordHash) 
                .HasColumnName("usu_senha");
                
            modelBuilder.Entity<ApplicationUser>().Ignore(t => t.PhoneNumber);
                
            modelBuilder.Entity<ApplicationUser>().Ignore(t => t.PhoneNumberConfirmed);
                
            modelBuilder.Entity<ApplicationUser>() 
                .Property(e => e.Roles) 
                .HasColumnName("usu_roles");
                
            modelBuilder.Entity<ApplicationUser>() 
                .Property(e => e.SecurityStamp) 
                .HasColumnName("usu_selo_seguranca");
                
            modelBuilder.Entity<ApplicationUser>() 
                .Property(e => e.TwoFactorEnabled) 
                .HasColumnName("usu_dois_passos_login");
                
            modelBuilder.Entity<ApplicationUser>() 
                .Property(e => e.UserName) 
                .HasColumnName("usu_nome");
                
            modelBuilder.Entity<ApplicationUser>().Property(t => t.UserName).IsRequired();
            
            modelBuilder.Entity<ApplicationUser>().Property(t => t.UserName).HasMaxLength(20);
            
            modelBuilder.Entity<ApplicationUser>() 
                .Property(e => e.Sobrenome) 
                .HasColumnName("usu_sobrenome");
                
            modelBuilder.Entity<ApplicationUser>().Property(t => t.Sobrenome).IsRequired();
            
            modelBuilder.Entity<ApplicationUser>().Property(t => t.Sobrenome).HasMaxLength(20);
            
            modelBuilder.Entity<ApplicationUser>() 
                .Property(e => e.DataNascimento) 
                .HasColumnName("usu_data_nascimento");
                
            modelBuilder.Entity<ApplicationUser>() 
                .Property(e => e.Sexo) 
                .HasColumnName("usu_sexo");
                
            modelBuilder.Entity<ApplicationUser>().Property(t => t.Sexo).IsRequired();
            
            modelBuilder.Entity<ApplicationUser>().Property(t => t.Sexo).IsMaxLength(1);
            
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Cartoes)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.UsuarioId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.DestinatarioMensagens)
                .WithOptional(e => e.UsuarioDestinatario)
                .HasForeignKey(e => e.UsuarioDestinatarioId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.RemetenteMensagens)
                .WithRequired(e => e.UsuarioRemetente)
                .HasForeignKey(e => e.UsuarioRemetenteId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Tasks)
                .WithRequired(e => e.UsuarioResponsavel)
                .HasForeignKey(e => e.UsuarioResponsavelId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Feedbacks)
                .WithRequired(e => e.UsuarioResponsavel)
                .HasForeignKey(e => e.UsuarioResponsavelId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Quests)
                .WithOptional(e => e.UsuarioCriador)
                .HasForeignKey(e => e.UsuarioCriadorId)
                .WillCascadeOnDelete();
            
            /*
                Configurando tabela Arquivo
            */
            
            //Configurando o nome da tabela
            modelBuilder.Entity<Arquivo>()
                .ToTable("arq_arquivo");
            
            //Configurando um campo como chave primária AUTO_INCREMENT
            modelBuilder.Entity<Arquivo>().Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOprion.Identity);
            
            //Configurando o nome do campo
            modelBuilder.Entity<Arquivo>() 
                .Property(e => e.Id) 
                .HasColumnName("arq_id");
            
            modelBuilder.Entity<Arquivo>()
                .Property(e => e.Name)
                .HasColumnName("arq_nome");
            
            //Configurando tamanho máximo da String
            modelBuilder.Entity<Arquivo>().Property(t => t.Nome).HasMaxLength(20);
            
            modelBuilder.Entity<Arquivo>()
                .Property(e => e.Path)
                .HasColumnName("arq_caminho");
            
            modelBuilder.Entity<Arquivo>().Property(t => t.Path).HasMaxLength(40);
            
            modelBuilder.Entity<Arquivo>()
                .Property(e => e.Size)
                .HasColumnName("arq_tamanho");
                
            modelBuilder.Entity<Arquivo>()
                .Property(e => e.UploadDate)
                .HasColumnName("arq_data_upload");
                
            modelBuilder.Entity<Arquivo>()
                .Property(e => e.UploadDate)
                .HasDefaultValue(DateTime.Now());
                
            modelBuilder.Entity<Arquivo>()
                .Property(e => e.VersaoAtual)
                .HasColumnName("arq_versao_atual");
                
            modelBuilder.Entity<Arquivo>()
                .Property(e => e.TaskId)
                .HasColumnName("task_id");
            
            /*
                Configurando tabela Cartao
            */
            
            modelBuilder.Entity<Cartao>()
                .ToTable("crt_cartao");
            
            modelBuilder.Entity<Cartao>()
                .Property(e => e.Id)
                .HasColumnName("crt_id");
            
            //Criando uma chave primária com AUTO_INCREMENT
            modelBuilder.Entity<Cartao>().Property(e => e.Id) 
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            
            modelBuilder.Entity<Cartao>()
                .Property(e => e.Id)
                .HasColumnName("crt_id");
                
            modelBuilder.Entity<Cartao>() 
                .Property(e => e.Bandeira) 
                .HasColumnName("crt_bandeira");
                
            modelBuilder.Entity<Cartao>().Property(t => t.Bandeira).IsRequired();
            
            modelBuilder.Entity<Cartao>() 
                .Property(e => e.Numero) 
                .HasColumnName("crt_numero");
                
            modelBuilder.Entity<Cartao>().Property(t => t.Numero).IsRequired();
            
            modelBuilder.Entity<Cartao>() 
                .Property(e => e.NomeTitular) 
                .HasColumnName("crt_nome_titular");
                
            modelBuilder.Entity<Cartao>().Property(t => t.NomeTitular).IsRequired();
            
            modelBuilder.Entity<Cartao>() 
                .Property(e => e.DataVencimento) 
                .HasColumnName("crt_data_vencimento");
                
            modelBuilder.Entity<Cartao>().Property(t => t.DataVencimento).IsRequired();
            
            modelBuilder.Entity<Cartao>() 
                .Property(e => e.CodigoSeguranca) 
                .HasColumnName("crt_codigo_seguranca");
                
            modelBuilder.Entity<Cartao>().Property(t => t.CodigoSeguranca).IsRequired();
            
            /*
                Configurando tabela ...
            */
            
            // ------------------- Coisas antigas ----------------------------------
            modelBuilder.Entity<Grupo>()
                .HasMany(e => e.Quests)
                .WithOptional(e => e.GrupoCriador)
                .HasForeignKey(e => e.GrupoCriadorId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Grupo>()
                .HasMany(e => e.Mensagens)
                .WithOptional(e => e.GrupoDestinatario)
                .HasForeignKey(e => e.GrupoDestinatarioId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Quest>()
                .HasMany(e => e.Antecedencias)
                .WithOptional(e => e.QuestAntecedente)
                .HasForeignKey(e => e.QuestAntecedenteId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Quest>()
                .HasMany(e => e.Precedencias)
                .WithOptional(e => e.QuestPrecedente)
                .HasForeignKey(e => e.QuestPrecedenteId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Quest>()
                .HasMany(e => e.Semanas)
                .WithMany(e => e.Quests)
                .Map(m => m.ToTable("qxs_quest_semana").MapLeftKey("qst_id").MapRightKey("sem_id"));

            modelBuilder.Entity<Task>()
                .HasMany(e => e.Antecedencias)
                .WithOptional(e => e.TaskAntecedente)
                .HasForeignKey(e => e.TaskAntecedenteId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Task>()
                .HasMany(e => e.Precedencias)
                .WithOptional(e => e.TaskPrecedente)
                .HasForeignKey(e => e.TaskPrecedenteId)
                .WillCascadeOnDelete();
        }
    }
}
