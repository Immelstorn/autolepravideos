using System.Data.Entity;
using System.Runtime.Remoting.Contexts;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Transactions;

namespace AutoLepraTop.DB.Models
{
    public class AutoLepraTopDbContext: DbContext
    {
        public AutoLepraTopDbContext(): base("AutoLepraVideosDb") { }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Post>().HasMany(p => p.Comments).WithRequired(c => c.Post);
        }

        public async Task SaveChangesWithIdentityAsync(string tableName)
        {
            using (var transaction = Database.BeginTransaction())
            {
                await Database.ExecuteSqlCommandAsync($"SET IDENTITY_INSERT [dbo].[{tableName}] ON");
                await SaveChangesAsync();
                await Database.ExecuteSqlCommandAsync($"SET IDENTITY_INSERT [dbo].[{tableName}] OFF");
                transaction.Commit();
            }
        }
    }
}