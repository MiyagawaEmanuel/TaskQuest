using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskQuest.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class DbContext : IdentityDbContext<User, Role, int,
        UserLogin, UserRole, UserClaim>
    {
        public DbContext()
            : base("DefaultConnection")
        {
            DbConfiguration.SetConfiguration(new MySqlEFConfiguration());
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DbContext>());
        }

        public virtual DbSet<Cartao> Cartao { get; set; }
        public virtual DbSet<Feedback> Feedback { get; set; }
        public virtual DbSet<Grupo> Grupo { get; set; }
        public virtual DbSet<Mensagem> Mensagen { get; set; }
        public virtual DbSet<Precedencia> Precedencia { get; set; }
        public virtual DbSet<Quest> Quest { get; set; }
        public virtual DbSet<Semana> Semana { get; set; }
        public virtual DbSet<Telefone> Telefone { get; set; }
        public virtual DbSet<Arquivo> Arquivo { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<ExperienciaUsuario> ExperienciaUsuario { get; set; }
        public virtual DbSet<Client> Client { get; set; }

        public static DbContext Create()
        {
            return new DbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            //Remove uma configuração embutida no Entity que modifica os nomes das tabelas e campos
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            /*
                Configurando tabela User (usu_usuario)
            */

            modelBuilder.Entity<User>()
                .ToTable("usu_usuario");

            modelBuilder.Entity<User>()
                .Property(e => e.AccessFailedCount)
                .HasColumnName("usu_contador_acesso_falho");

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .HasColumnName("usu_email")
                .HasMaxLength(40)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new[]
                    {
                        new IndexAttribute("usu_email_unique_idx") { IsUnique = true }
                    }));

            modelBuilder.Entity<User>()
                .Property(e => e.EmailConfirmed)
                .HasColumnName("usu_email_confirmado");

            modelBuilder.Entity<User>()
                .HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("usu_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<User>()
                .Property(e => e.LockoutEnabled)
                .HasColumnName("usu_bloqueado");

            modelBuilder.Entity<User>()
                .Property(e => e.LockoutEndDateUtc)
                .HasColumnName("usu_data_desbloqueio");

            modelBuilder.Entity<User>()
                .Property(e => e.PasswordHash)
                .HasColumnName("usu_senha");

            modelBuilder.Entity<User>().Ignore(e => e.PhoneNumber);

            modelBuilder.Entity<User>().Ignore(e => e.PhoneNumberConfirmed);

            modelBuilder.Entity<User>().Ignore(e => e.CurrentClientId);

            modelBuilder.Entity<User>()
                .Property(e => e.SecurityStamp)
                .HasColumnName("usu_selo_seguranca");

            modelBuilder.Entity<User>()
                .Property(e => e.TwoFactorEnabled)
                .HasColumnName("usu_dois_passos_login");

            modelBuilder.Entity<User>()
                .Property(e => e.UserName)
                .HasColumnName("usu_user_name")
                .IsRequired()
                .HasMaxLength(40);

            modelBuilder.Entity<User>()
                .Property(e => e.UserName)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new[]
                    {
                        new IndexAttribute("usu_user_name_unique_idx") { IsUnique = true }
                    }));

            modelBuilder.Entity<User>()
                .Property(e => e.Nome)
                .HasColumnName("usu_nome")
                .HasMaxLength(40)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(e => e.Sobrenome)
                .HasColumnName("usu_sobrenome")
                .IsRequired()
                .HasMaxLength(40);
            
            modelBuilder.Entity<User>()
                .Property(e => e.DataNascimento)
                .HasColumnName("usu_data_nascimento");

            modelBuilder.Entity<User>()
                .Property(e => e.Sexo)
                .HasColumnName("usu_sexo")
                .IsRequired()
                .HasMaxLength(1);

            modelBuilder.Entity<User>()
                .Property(e => e.Cor)
                .HasColumnName("usu_cor")
                .HasMaxLength(7)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasMany(e => e.Cartoes)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.UsuarioId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Clients)
                .WithOptional(e => e.Usuario)
                .HasForeignKey(e => e.UsuarioId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<User>()
                .HasMany(e => e.DestinatarioMensagens)
                .WithOptional(e => e.UsuarioDestinatario)
                .HasForeignKey(e => e.UsuarioDestinatarioId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<User>()
                .HasMany(e => e.RemetenteMensagens)
                .WithRequired(e => e.UsuarioRemetente)
                .HasForeignKey(e => e.UsuarioRemetenteId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Feedbacks)
                .WithOptional(e => e.UsuarioResponsavel)
                .HasForeignKey(e => e.UsuarioResponsavelId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Quests)
                .WithOptional(e => e.UsuarioCriador)
                .HasForeignKey(e => e.UsuarioCriadorId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Tasks)
                .WithOptional(e => e.UsuarioResponsavel)
                .HasForeignKey(e => e.UsuarioResponsavelId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Grupos)
                .WithMany(e => e.Users)
                .Map(m => m.ToTable("uxg_usuario_grupo").MapLeftKey("usu_id").MapRightKey("gru_id"));

            /*
                Configurando tabela Arquivo
            */

            //Configurando o nome da tabela
            modelBuilder.Entity<Arquivo>()
                .ToTable("arq_arquivo");

            //Configurando um campo como chave primária AUTO_INCREMENT
            modelBuilder.Entity<Arquivo>()
                .HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("arq_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Arquivo>()
                .Property(e => e.Nome)
                .HasColumnName("arq_nome")
                .HasMaxLength(40);

            modelBuilder.Entity<Arquivo>()
                .Property(e => e.Path)
                .HasColumnName("arq_caminho")
                .HasMaxLength(120);

            modelBuilder.Entity<Arquivo>()
                .Property(e => e.Size)
                .HasColumnName("arq_tamanho");

            modelBuilder.Entity<Arquivo>()
                .Property(e => e.UploadDate)
                .HasColumnName("arq_data_upload")
                .IsRequired();

            modelBuilder.Entity<Arquivo>()
                .Property(e => e.VersaoAtual)
                .HasColumnName("arq_versao_atual");

            modelBuilder.Entity<Arquivo>()
                .Property(e => e.TaskId)
                .HasColumnName("tsk_id");

            /*
                Configurando tabela Cartao
            */

            modelBuilder.Entity<Cartao>()
                .ToTable("crt_cartao");

            modelBuilder.Entity<Cartao>()
                .HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("crt_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Cartao>()
                .Property(e => e.Bandeira)
                .HasColumnName("crt_bandeira")
                .IsRequired();

            modelBuilder.Entity<Cartao>()
                .Property(e => e.Numero)
                .HasColumnName("crt_numero")
                .IsRequired();

            modelBuilder.Entity<Cartao>()
                .Property(e => e.NomeTitular)
                .HasColumnName("crt_nome_titular")
                .IsRequired();

            modelBuilder.Entity<Cartao>()
                .Property(e => e.DataVencimento)
                .HasColumnName("crt_data_vencimento")
                .IsRequired();

            modelBuilder.Entity<Cartao>()
                .Property(e => e.CodigoSeguranca)
                .HasColumnName("crt_codigo_seguranca")
                .IsRequired();

            modelBuilder.Entity<Cartao>()
                .Property(e => e.UsuarioId)
                .HasColumnName("usu_id");

            /*
                Configurando tabela Role
            */

            modelBuilder.Entity<Role>()
                .ToTable("ctr_custom_role");

            modelBuilder.Entity<Role>()
                .HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("ctr_Id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Role>()
                .Property(e => e.Name)
                .HasColumnName("ctr_name");

            /*
                Configurando tabela UserClaim 
            */

            modelBuilder.Entity<UserClaim>()
                .ToTable("cuc_custom_user_claim");

            modelBuilder.Entity<UserClaim>()
                .HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("cuc_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<UserClaim>()
                .Property(e => e.UserId)
                .HasColumnName("usu_id");

            modelBuilder.Entity<UserClaim>()
                .Property(e => e.ClaimType)
                .HasColumnName("cuc_type");

            modelBuilder.Entity<UserClaim>()
                .Property(e => e.ClaimValue)
                .HasColumnName("cuc_value");

            /*
                Configurando tabela UserLogin 
            */

            modelBuilder.Entity<UserLogin>()
                .ToTable("cul_custom_user_login");

            modelBuilder.Entity<UserLogin>()
                .HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("cul_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<UserLogin>()
                .Property(e => e.UserId)
                .HasColumnName("usu_id");

            modelBuilder.Entity<UserLogin>()
                .Property(e => e.LoginProvider)
                .HasColumnName("cul_login_provider");

            modelBuilder.Entity<UserLogin>()
                .Property(e => e.ProviderKey)
                .HasColumnName("cul_provider_key");

            /*
                Configurando tabela UserRole 
            */

            modelBuilder.Entity<UserRole>()
                .ToTable("cur_custom_user_role");

            modelBuilder.Entity<UserRole>()
                .HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("cur_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<UserRole>()
                .Property(e => e.UserId)
                .HasColumnName("usu_id");

            modelBuilder.Entity<UserRole>()
                .Property(e => e.RoleId)
                .HasColumnName("rol_id");

            /*
                Configurando a tabela Client 
            */

            modelBuilder.Entity<Client>()
                .ToTable("cli_client");

            modelBuilder.Entity<Client>()
                .HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("cli_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Client>()
                .Property(e => e.ClientKey)
                .HasColumnName("cli_key");

            modelBuilder.Entity<Client>()
                .Property(e => e.UsuarioId)
                .HasColumnName("usu_id");

            /*
                Configurando tabela Feedback
            */

            modelBuilder.Entity<Feedback>()
                .ToTable("feb_feedback");

            modelBuilder.Entity<Feedback>()
                .HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("feb_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Feedback>()
                .Property(e => e.Relatorio)
                .HasColumnName("feb_relatorio")
                .HasMaxLength(120);

            modelBuilder.Entity<Feedback>()
                .Property(e => e.Resposta)
                .HasColumnName("feb_resposta")
                .HasMaxLength(120);

            modelBuilder.Entity<Feedback>()
                .Property(e => e.Nota)
                .HasColumnName("feb_nota")
                .IsRequired();

            modelBuilder.Entity<Feedback>()
                .Property(e => e.DataCriacao)
                .HasColumnName("feb_data_criacao")
                .IsRequired();

            modelBuilder.Entity<Feedback>()
                .Property(e => e.TaskId)
                .HasColumnName("tsk_id")
                .IsRequired();

            modelBuilder.Entity<Feedback>()
                .Property(e => e.UsuarioResponsavelId)
                .HasColumnName("usu_id_responsavel");

            /*
                Configurando a tabela Grupo 
            */

            modelBuilder.Entity<Grupo>()
                .ToTable("gru_grupo");

            modelBuilder.Entity<Grupo>()
                .HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("gru_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Grupo>()
                .Property(e => e.Nome)
                .HasColumnName("gru_nome")
                .HasMaxLength(40)
                .IsRequired();

            modelBuilder.Entity<Grupo>()
                .Property(e => e.Cor)
                .HasColumnName("gru_cor")
                .HasMaxLength(7)
                .IsRequired();

            modelBuilder.Entity<Grupo>()
                .Property(e => e.DataCriacao)
                .HasColumnName("gru_data_criacao")
                .IsRequired();

            modelBuilder.Entity<Grupo>()
                .Property(e => e.Plano)
                .HasColumnName("gru_plano")
                .IsRequired();

            modelBuilder.Entity<Grupo>()
                .Property(e => e.Descricao)
                .HasColumnName("gru_descricao")
                .HasMaxLength(120)
                .IsRequired();

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

            /*
                 Configurando tabela Mensagem
            */

            modelBuilder.Entity<Mensagem>()
                .ToTable("msg_mensagem");

            modelBuilder.Entity<Mensagem>()
                .HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("msg_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Mensagem>()
                .Property(e => e.UsuarioRemetenteId)
                .HasColumnName("usu_id_remetente")
                .IsRequired();

            modelBuilder.Entity<Mensagem>()
                .Property(e => e.UsuarioDestinatarioId)
                .HasColumnName("usu_id_destinatario");

            modelBuilder.Entity<Mensagem>()
                .Property(e => e.GrupoDestinatarioId)
                .HasColumnName("gru_id_destinatario");

            modelBuilder.Entity<Mensagem>()
                .Property(e => e.Conteudo)
                .HasColumnName("msg_conteudo")
                .HasMaxLength(120).IsRequired();


            modelBuilder.Entity<Mensagem>()
                .Property(e => e.Data)
                .HasColumnName("msg_data")
                .IsRequired();

            /*
                Configuração da tabela Precedencia 
            */

            modelBuilder.Entity<Precedencia>()
                .ToTable("pre_precedencia");

            modelBuilder.Entity<Precedencia>()
                .HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("pre_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Precedencia>()
                .Property(e => e.QuestAntecedenteId)
                .HasColumnName("qst_id_antecedente");

            modelBuilder.Entity<Precedencia>()
                .Property(e => e.TaskAntecedenteId)
                .HasColumnName("tsk_id_antecedente");

            modelBuilder.Entity<Precedencia>()
                .Property(e => e.QuestPrecedenteId)
                .HasColumnName("qst_id_precedente");

            modelBuilder.Entity<Precedencia>()
                .Property(e => e.TaskPrecedenteId)
                .HasColumnName("tsk_id_precedente");

            /*
                Configuração da tabela Quest 
            */

            modelBuilder.Entity<Quest>()
                .ToTable("qst_quest");

            modelBuilder.Entity<Quest>()
                .HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("qst_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Quest>()
                .Property(e => e.UsuarioCriadorId)
                .HasColumnName("usu_id_criador");

            modelBuilder.Entity<Quest>()
                .Property(e => e.GrupoCriadorId)
                .HasColumnName("gru_id_criador");

            modelBuilder.Entity<Quest>()
                .Property(e => e.Cor)
                .HasColumnName("qst_cor")
                .HasMaxLength(7)
                .IsRequired();

            modelBuilder.Entity<Quest>()
                .Property(e => e.DataCriacao)
                .HasColumnName("qst_data_criacao")
                .IsRequired();

            modelBuilder.Entity<Quest>()
                .Property(e => e.Descricao)
                .HasColumnName("qst_descricao")
                .HasMaxLength(120)
                .IsRequired();

            modelBuilder.Entity<Quest>()
                .Property(e => e.Nome)
                .HasColumnName("qst_nome")
                .HasMaxLength(40)
                .IsRequired();

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

            /*
                Configuração da tabela Semana 
            */

            modelBuilder.Entity<Semana>()
                .ToTable("sem_semana");

            modelBuilder.Entity<Semana>()
                .HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("sem_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Semana>()
                .Property(e => e.Dia)
                .HasColumnName("sem_dia")
                .HasMaxLength(10)
                .IsRequired();

            modelBuilder.Entity<Semana>()
                .Property(e => e.Sigla)
                .HasColumnName("sem_sigla")
                .HasMaxLength(1)
                .IsRequired();

            /*
                Configuração da tabela Telefone 
            */

            modelBuilder.Entity<Telefone>()
                .ToTable("tel_telefone");

            modelBuilder.Entity<Telefone>()
                .HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("tel_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Telefone>()
                .Property(e => e.UsuarioId)
                .HasColumnName("usu_id")
                .IsRequired();

            modelBuilder.Entity<Telefone>()
                .Property(e => e.Numero)
                .HasColumnName("tel_numero")
                .IsRequired();

            modelBuilder.Entity<Telefone>()
                .Property(e => e.Tipo)
                .HasColumnName("tel_tipo")
                .HasMaxLength(40)
                .IsRequired();

            /*
                Configuração da tabela Task 
            */

            modelBuilder.Entity<Task>()
                .ToTable("tsk_task");

            modelBuilder.Entity<Task>()
                .HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasColumnName("tsk_id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Task>()
                .Property(e => e.QuestId)
                .HasColumnName("qst_id")
                .IsRequired();

            modelBuilder.Entity<Task>()
                .Property(e => e.Nome)
                .HasColumnName("tsk_nome")
                .HasMaxLength(40)
                .IsRequired();

            modelBuilder.Entity<Task>()
                .Property(e => e.Descricao)
                .HasColumnName("tsk_descricao")
                .IsRequired();

            modelBuilder.Entity<Task>()
                .Property(e => e.Status)
                .HasColumnName("tsk_status")
                .IsRequired();

            modelBuilder.Entity<Task>()
                .Property(e => e.Dificuldade)
                .HasColumnName("tsk_dificuldade")
                .IsRequired();

            modelBuilder.Entity<Task>()
                .Property(e => e.DataCriacao)
                .HasColumnName("tsk_data_criacao")
                .IsRequired();

            modelBuilder.Entity<Task>()
                .Property(e => e.DataConclusao)
                .HasColumnName("tsk_data_conclusao")
                .IsRequired();

            modelBuilder.Entity<Task>()
                .Property(e => e.RequerVerificacao)
                .HasColumnName("tsk_verificacao")
                .IsRequired();

            modelBuilder.Entity<Task>()
                .Property(e => e.UsuarioResponsavelId)
                .HasColumnName("usu_id_responsavel");

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

            /*
                Configuração da tabela ExperienciaUsuario 
            */

            modelBuilder.Entity<ExperienciaUsuario>()
                .ToTable("xpu_experiencia_usuario");

            modelBuilder.Entity<ExperienciaUsuario>()
                .Property(e => e.TaskId)
                .HasColumnName("tsk_id");

            modelBuilder.Entity<ExperienciaUsuario>()
                .Property(e => e.UsuarioId)
                .HasColumnName("usu_id");

            modelBuilder.Entity<ExperienciaUsuario>()
                .HasKey(e => new { e.TaskId, e.UsuarioId });

            modelBuilder.Entity<ExperienciaUsuario>()
                .Property(e => e.Valor)
                .HasColumnName("xpu_valor")
                .IsRequired();

            modelBuilder.Entity<ExperienciaUsuario>()
                .Property(e => e.Data)
                .HasColumnName("xpu_data")
                .IsRequired();

        }
    }
}
