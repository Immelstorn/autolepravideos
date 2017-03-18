namespace AutoLepraTop.DB.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AutoLepraTop.DB.Models.AutoLepraTopDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AutoLepraTop.DB.Models.AutoLepraTopDbContext context)
        {
          
        }
    }
}
