using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskQuest.Data.EntityConfiguration;
using System.Text;
using TaskQuest.Models;
using System.Linq;
using System;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Collections.Generic;

namespace TaskQuest.Data
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

        public virtual DbSet<Backup> Backup { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Feedback> Feedback { get; set; }
        public virtual DbSet<File> File { get; set; }
        public virtual DbSet<Grupo> Grupo { get; set; }
        public virtual DbSet<Mensagem> Mensagen { get; set; }
        public virtual DbSet<Notificacao> Notificacao { get; set; }
        public virtual DbSet<Pagamento> Pagamento { get; set; }
        public virtual DbSet<PontoUsuario> PontoUsuario { get; set; }
        public virtual DbSet<Quest> Quest { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<Telefone> Telefone { get; set; }

        public static DbContext Create()
        {
            return new DbContext();
        }

        public override int SaveChanges()
        {
            //System.Threading.Tasks.Task.Run(() => CreateBackupAsync(ChangeTracker.Entries()));
            //System.Threading.Tasks.Task.Run(() => CreateNotificacaoAsync(ChangeTracker.Entries()));
            return base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            //Remove uma configuração embutida no Entity que modifica os nomes das tabelas e campos
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new BackupConfiguration());
            modelBuilder.Configurations.Add(new ClientConfiguration());
            modelBuilder.Configurations.Add(new FeedbackConfiguration());
            modelBuilder.Configurations.Add(new FileConfiguration());
            modelBuilder.Configurations.Add(new GrupoConfiguration());
            modelBuilder.Configurations.Add(new MensagemConfiguration());
            modelBuilder.Configurations.Add(new NotificacaoConfiguration());
            modelBuilder.Configurations.Add(new PagamentoConfiguration());
            modelBuilder.Configurations.Add(new PontoUsuarioConfiguration());
            modelBuilder.Configurations.Add(new QuestConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new TaskConfiguration());
            modelBuilder.Configurations.Add(new TelefoneConfiguration());
            modelBuilder.Configurations.Add(new UserClaimConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserLoginConfiguration());
            modelBuilder.Configurations.Add(new UserRoleConfiguration());
        }

        private async void CreateNotificacaoAsync(IEnumerable<DbEntityEntry> entries)
        {
            foreach (var entry in entries)
            {
                if (entry.State != EntityState.Unchanged)
                {

                    if (entry.Entity is NotificacaoMetaData)
                    {

                        var notificacao = new Notificacao();
                        bool IsValid = true;

                        notificacao.TipoNotificacao = entry.State;
                        notificacao.EntidadeModificada = entry.Entity.GetType().ToString();
                        notificacao.DataNotificacao = DateTime.Now;

                        if (entry.Entity.GetType() == typeof(Grupo))
                        {
                            var grupo = ((Grupo)entry.Entity);
                            notificacao.GrupoId = grupo.Id;
                            notificacao.Texto = "";
                        }
                        else if (entry.Entity.GetType() == typeof(Quest))
                        {
                            var quest = ((Quest)entry.Entity);
                            if (quest.GrupoCriadorId != null)
                            {
                                notificacao.GrupoId = quest.GrupoCriadorId.Value;
                                notificacao.Texto = "";
                            }
                                
                            else
                                IsValid = false;
                        }
                        else if (entry.Entity.GetType() == typeof(Task))
                        {
                            var task = ((Task)entry.Entity);
                            if (task.Quest.GrupoCriadorId != null)
                            {
                                notificacao.GrupoId = task.Quest.GrupoCriadorId.Value;
                                notificacao.Texto = "";
                            }
                            else
                                IsValid = false;
                        }
                        else if (entry.Entity.GetType() == typeof(Feedback))
                        {
                            var feedback = ((Feedback)entry.Entity);
                            if (feedback.Task.Quest.GrupoCriadorId != null)
                            {
                                notificacao.GrupoId = feedback.Task.Quest.GrupoCriadorId.Value;
                                notificacao.Texto = "";
                            }
                            else
                                IsValid = false;
                        }
                        if (IsValid)
                            this.Notificacao.Add(notificacao);
                    }
                }
            }
            base.SaveChanges();
        }

        private async void CreateBackupAsync(IEnumerable<DbEntityEntry> entries)
        {
            foreach (var entry in entries)
            {
                if (entry.State != EntityState.Unchanged)
                {
                    var bkp = new Backup();

                    bkp.TableName = entry.Entity.GetType().ToString();
                    bkp.QueryType = entry.State.ToString();

                    StringBuilder data = new StringBuilder();

                    foreach (var prop in entry.Entity.GetType().GetProperties())
                    {
                        if (data.Length > 0)
                            data.Append("&");
                        data.Append(prop.Name);
                        data.Append("=");
                        data.Append(prop.GetValue(entry.Entity));
                    }

                    bkp.Data = data.ToString();
                    this.Backup.Add(bkp);
                }
                base.SaveChanges();
            }
        }

    }
}
