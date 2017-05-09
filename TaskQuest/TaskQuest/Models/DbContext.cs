using System.Data.Entity;
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
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Cartao>()
                .Property(e => e.Bandeira)
                .IsUnicode(false);

            modelBuilder.Entity<Cartao>()
                .Property(e => e.NomeTitular)
                .IsUnicode(false);

            modelBuilder.Entity<Cartao>()
                .Property(e => e.CodigoSeguranca)
                .IsUnicode(false);

            modelBuilder.Entity<Feedback>()
                .Property(e => e.Relatorio)
                .IsUnicode(false);

            modelBuilder.Entity<Feedback>()
                .Property(e => e.Resposta)
                .IsUnicode(false);

            modelBuilder.Entity<Grupo>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Grupo>()
                .Property(e => e.Cor)
                .IsUnicode(false);

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

            modelBuilder.Entity<Mensagem>()
                .Property(e => e.Conteudo)
                .IsUnicode(false);

            modelBuilder.Entity<Quest>()
                .Property(e => e.Cor)
                .IsUnicode(false);

            modelBuilder.Entity<Quest>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Quest>()
                .Property(e => e.Nome)
                .IsUnicode(false);

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

            modelBuilder.Entity<Semana>()
                .Property(e => e.Dia)
                .IsUnicode(false);

            modelBuilder.Entity<Semana>()
                .Property(e => e.Sigla)
                .IsUnicode(false);

            modelBuilder.Entity<Telefone>()
                .Property(e => e.Tipo)
                .IsUnicode(false);

            modelBuilder.Entity<Arquivo>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Arquivo>()
                .Property(e => e.Path)
                .IsUnicode(false);

            modelBuilder.Entity<Task>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Task>()
                .Property(e => e.Duracao)
                .HasPrecision(2);

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

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.Sobrenome)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.Sexo)
                .IsUnicode(false);

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
        }
    }
}