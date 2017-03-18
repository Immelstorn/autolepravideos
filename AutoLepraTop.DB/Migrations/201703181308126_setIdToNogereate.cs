namespace AutoLepraTop.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class setIdToNogereate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "Post_Id", "dbo.Posts");
            DropPrimaryKey("dbo.Comments");
            DropPrimaryKey("dbo.Posts");
            AlterColumn("dbo.Comments", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Posts", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Comments", "Id");
            AddPrimaryKey("dbo.Posts", "Id");
            AddForeignKey("dbo.Comments", "Post_Id", "dbo.Posts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "Post_Id", "dbo.Posts");
            DropPrimaryKey("dbo.Posts");
            DropPrimaryKey("dbo.Comments");
            AlterColumn("dbo.Posts", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Comments", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Posts", "Id");
            AddPrimaryKey("dbo.Comments", "Id");
            AddForeignKey("dbo.Comments", "Post_Id", "dbo.Posts", "Id", cascadeDelete: true);
        }
    }
}
