﻿using System.Configuration;
using System.Data.Entity;

namespace AutoLepraTop.DB.Models
{
    public class AutoLepraTopDbContext: DbContext
    {
        private static string _connString = ConfigurationManager.AppSettings["AzureDbConnection"];
        public AutoLepraTopDbContext(): base(_connString) { }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Comment>().HasRequired(c => c.Post).WithMany(p => p.Comments).HasForeignKey(c => c.Post_Id);
        }
    }
}