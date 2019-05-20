namespace LoadDist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeLoadModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Loads", "LectureHours", c => c.Int(nullable: false));
            AddColumn("dbo.Loads", "LabsHours", c => c.Int(nullable: false));
            AddColumn("dbo.Loads", "PracticalHours", c => c.Int(nullable: false));
            AddColumn("dbo.Loads", "ExamHours", c => c.Int(nullable: false));
            AddColumn("dbo.Loads", "TestHours", c => c.Int(nullable: false));
            AddColumn("dbo.Loads", "Year", c => c.Int(nullable: false));
            AddColumn("dbo.Loads", "Term", c => c.Int(nullable: false));
            AddColumn("dbo.Loads", "SyllabusContent_Id", c => c.Int());
            CreateIndex("dbo.Loads", "SyllabusContent_Id");
            AddForeignKey("dbo.Loads", "SyllabusContent_Id", "dbo.SyllabusContents", "Id");
            DropColumn("dbo.Loads", "StreamsCount");
            DropColumn("dbo.Loads", "SubgroupsCount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Loads", "SubgroupsCount", c => c.Int(nullable: false));
            AddColumn("dbo.Loads", "StreamsCount", c => c.Int(nullable: false));
            DropForeignKey("dbo.Loads", "SyllabusContent_Id", "dbo.SyllabusContents");
            DropIndex("dbo.Loads", new[] { "SyllabusContent_Id" });
            DropColumn("dbo.Loads", "SyllabusContent_Id");
            DropColumn("dbo.Loads", "Term");
            DropColumn("dbo.Loads", "Year");
            DropColumn("dbo.Loads", "TestHours");
            DropColumn("dbo.Loads", "ExamHours");
            DropColumn("dbo.Loads", "PracticalHours");
            DropColumn("dbo.Loads", "LabsHours");
            DropColumn("dbo.Loads", "LectureHours");
        }
    }
}
