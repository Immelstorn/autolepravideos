using System.Data.Entity;
using System.Security.AccessControl;

namespace AutoLepraTop.DB.Models
{
    public class AutoLepraTopDbContext:DbContext
    {
        public AutoLepraTopDbContext():base("AutoLepraVideosDb")
        {
            
        }

       public DbSet<Post> Posts { get; set; }
    }
}