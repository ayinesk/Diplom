namespace LoadDist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewLoadProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Loads", "Group_Id", c => c.Int());
            AddColumn("dbo.Loads", "Stream_Id", c => c.Int());
            CreateIndex("dbo.Loads", "Group_Id");
            CreateIndex("dbo.Loads", "Stream_Id");
            AddForeignKey("dbo.Loads", "Group_Id", "dbo.Groups", "Id");
            AddForeignKey("dbo.Loads", "Stream_Id", "dbo.Streams", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Loads", "Stream_Id", "dbo.Streams");
            DropForeignKey("dbo.Loads", "Group_Id", "dbo.Groups");
            DropIndex("dbo.Loads", new[] { "Stream_Id" });
            DropIndex("dbo.Loads", new[] { "Group_Id" });
            DropColumn("dbo.Loads", "Stream_Id");
            DropColumn("dbo.Loads", "Group_Id");
        }
    }
}
