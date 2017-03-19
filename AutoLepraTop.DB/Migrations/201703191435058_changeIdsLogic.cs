namespace AutoLepraTop.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeIdsLogic : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "Post_Id", "dbo.Posts");
            DropPrimaryKey("dbo.Comments");
            DropPrimaryKey("dbo.Posts");
            AddColumn("dbo.Comments", "LepraId", c => c.Int(nullable: false));
            AddColumn("dbo.Posts", "LepraId", c => c.Int(nullable: false));
            AlterColumn("dbo.Comments", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Posts", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Comments", "Id");
            AddPrimaryKey("dbo.Posts", "Id");
            AddForeignKey("dbo.Comments", "Post_Id", "dbo.Posts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "Post_Id", "dbo.Posts");
            DropPrimaryKey("dbo.Posts");
            DropPrimaryKey("dbo.Comments");
            AlterColumn("dbo.Posts", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Comments", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.Posts", "LepraId");
            DropColumn("dbo.Comments", "LepraId");
            AddPrimaryKey("dbo.Posts", "Id");
            AddPrimaryKey("dbo.Comments", "Id");
            AddForeignKey("dbo.Comments", "Post_Id", "dbo.Posts", "Id", cascadeDelete: true);
        }
    }
}
