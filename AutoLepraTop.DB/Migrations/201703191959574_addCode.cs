namespace AutoLepraTop.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "VideoCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "VideoCode");
        }
    }
}
