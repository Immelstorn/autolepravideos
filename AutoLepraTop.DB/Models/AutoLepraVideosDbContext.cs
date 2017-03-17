using System.Data.Entity;

namespace AutoLepraTop.DB.Models
{
    public class AutoLepraVideosDbContext:DbContext
    {
        public AutoLepraVideosDbContext():base("AutoLepraVideosDb")
        {
            
        }
    }
}