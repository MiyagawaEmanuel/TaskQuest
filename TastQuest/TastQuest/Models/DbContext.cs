using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using TastQuest.Models;

namespace TaskQuest.Models
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class DbContext : IdentityDbContext<ApplicationUser, CustomRole, int,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public DbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<DbContext>());
            DbConfiguration.SetConfiguration(new MySql.Data.Entity.MySqlEFConfiguration());

        }

        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<crt_cartao> crt_cartao { get; set; }
        public virtual DbSet<feb_feedback> feb_feedback { get; set; }
        public virtual DbSet<gru_grupo> gru_grupo { get; set; }
        public virtual DbSet<msg_mensagem> msg_mensagem { get; set; }
        public virtual DbSet<pre_precedencia> pre_precedencia { get; set; }
        public virtual DbSet<qst_quest> qst_quest { get; set; }
        public virtual DbSet<sem_semana> sem_semana { get; set; }
        public virtual DbSet<tel_telefone> tel_telefone { get; set; }
        public virtual DbSet<arq_arquivos> arq_arquivos { get; set; }
        public virtual DbSet<tsk_task> tsk_task { get; set; }
        public virtual DbSet<uxg_usuario_grupo> uxg_usuario_grupo { get; set; }
        public virtual DbSet<xpg_experiencia_grupo> xpg_experiencia_grupo { get; set; }
        public virtual DbSet<xpu_experiencia_usuario> xpu_experiencia_usuario { get; set; }
        public virtual DbSet<Claims> Claims { get; set; }

        public static DbContext Create()
        {
            return new DbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<crt_cartao>()
                .Property(e => e.crt_bandeira)
                .IsUnicode(false);

            modelBuilder.Entity<crt_cartao>()
                .Property(e => e.crt_nome_titular)
                .IsUnicode(false);

            modelBuilder.Entity<crt_cartao>()
                .Property(e => e.crt_codigo_seguranca)
                .IsUnicode(false);

            modelBuilder.Entity<feb_feedback>()
                .Property(e => e.feb_relatorio)
                .IsUnicode(false);

            modelBuilder.Entity<feb_feedback>()
                .Property(e => e.feb_feedback1)
                .IsUnicode(false);

            modelBuilder.Entity<gru_grupo>()
                .Property(e => e.gru_nome)
                .IsUnicode(false);

            modelBuilder.Entity<gru_grupo>()
                .Property(e => e.gru_cor)
                .IsUnicode(false);

            modelBuilder.Entity<gru_grupo>()
                .HasMany(e => e.qst_quest)
                .WithOptional(e => e.gru_grupo)
                .HasForeignKey(e => e.gru_id_criador)
                .WillCascadeOnDelete();

            modelBuilder.Entity<gru_grupo>()
                .HasMany(e => e.msg_mensagem)
                .WithOptional(e => e.gru_grupo)
                .HasForeignKey(e => e.gru_id_destinatario)
                .WillCascadeOnDelete();

            modelBuilder.Entity<msg_mensagem>()
                .Property(e => e.msg_conteudo)
                .IsUnicode(false);

            modelBuilder.Entity<qst_quest>()
                .Property(e => e.qst_cor)
                .IsUnicode(false);

            modelBuilder.Entity<qst_quest>()
                .Property(e => e.qst_descricao)
                .IsUnicode(false);

            modelBuilder.Entity<qst_quest>()
                .Property(e => e.qst_nome)
                .IsUnicode(false);

            modelBuilder.Entity<qst_quest>()
                .HasMany(e => e.pre_precedencia)
                .WithOptional(e => e.qst_quest)
                .HasForeignKey(e => e.qst_id_antecedente)
                .WillCascadeOnDelete();

            modelBuilder.Entity<qst_quest>()
                .HasMany(e => e.pre_precedencia1)
                .WithOptional(e => e.qst_quest1)
                .HasForeignKey(e => e.qst_id_precedente)
                .WillCascadeOnDelete();

            modelBuilder.Entity<qst_quest>()
                .HasMany(e => e.sem_semana)
                .WithMany(e => e.qst_quest)
                .Map(m => m.ToTable("qxs_quest_semana", "taskquest").MapLeftKey("qst_id").MapRightKey("sem_id"));

            modelBuilder.Entity<sem_semana>()
                .Property(e => e.sem_dia)
                .IsUnicode(false);

            modelBuilder.Entity<sem_semana>()
                .Property(e => e.sem_sigla)
                .IsUnicode(false);

            modelBuilder.Entity<tel_telefone>()
                .Property(e => e.tel_tipo)
                .IsUnicode(false);

            modelBuilder.Entity<arq_arquivos>()
                .Property(e => e.arq_nome)
                .IsUnicode(false);

            modelBuilder.Entity<arq_arquivos>()
                .Property(e => e.arq_caminho)
                .IsUnicode(false);

            modelBuilder.Entity<tsk_task>()
                .Property(e => e.tsk_nome)
                .IsUnicode(false);

            modelBuilder.Entity<tsk_task>()
                .Property(e => e.tsk_duracao)
                .HasPrecision(2);

            modelBuilder.Entity<tsk_task>()
                .HasMany(e => e.pre_precedencia)
                .WithOptional(e => e.tsk_task)
                .HasForeignKey(e => e.tsk_id_antecedente)
                .WillCascadeOnDelete();

            modelBuilder.Entity<tsk_task>()
                .HasMany(e => e.pre_precedencia1)
                .WithOptional(e => e.tsk_task1)
                .HasForeignKey(e => e.tsk_id_precedente)
                .WillCascadeOnDelete();

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.usu_nome)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.usu_sobrenome)
                .IsUnicode(false);
            
            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.usu_sexo)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.crt_cartao)
                .WithRequired(e => e.usu_usuario)
                .HasForeignKey(e => e.usu_usuario_usu_id);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.msg_mensagem)
                .WithOptional(e => e.usu_usuario)
                .HasForeignKey(e => e.usu_id_destinatario)
                .WillCascadeOnDelete();

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.msg_mensagem1)
                .WithRequired(e => e.usu_usuario1)
                .HasForeignKey(e => e.usu_id_remetente);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.pre_precedencia)
                .WithRequired(e => e.usu_usuario)
                .HasForeignKey(e => e.usu_id_responsavel_conclusao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.qst_quest)
                .WithOptional(e => e.usu_usuario)
                .HasForeignKey(e => e.usu_id_criador)
                .WillCascadeOnDelete();

        }

    }
}