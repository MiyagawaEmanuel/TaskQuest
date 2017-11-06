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
            foreach (var entry in ChangeTracker.Entries())
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
                    Backup.Add(bkp);
                }
            }

            bool saveFailed;
            do
            {
                saveFailed = false;
                try
                {
                    return base.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update original values from the database 
                    var entry = ex.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                }

            } while (saveFailed);
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
    }
}
