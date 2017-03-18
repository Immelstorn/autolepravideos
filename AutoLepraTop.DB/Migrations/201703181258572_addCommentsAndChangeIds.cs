namespace AutoLepraTop.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCommentsAndChangeIds : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Body = c.String(),
                        Link = c.String(),
                        Rating = c.Int(nullable: false),
                        Post_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.Post_Id, cascadeDelete: true)
                .Index(t => t.Post_Id);
            
            DropColumn("dbo.Posts", "PostId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "PostId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Comments", "Post_Id", "dbo.Posts");
            DropIndex("dbo.Comments", new[] { "Post_Id" });
            DropTable("dbo.Comments");
        }
    }
}
