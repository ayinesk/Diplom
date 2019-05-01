namespace LoadDist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubjectLecturers",
                c => new
                    {
                        Subject_Id = c.Int(nullable: false),
                        Lecturer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Subject_Id, t.Lecturer_Id })
                .ForeignKey("dbo.Subjects", t => t.Subject_Id, cascadeDelete: true)
                .ForeignKey("dbo.Lecturers", t => t.Lecturer_Id, cascadeDelete: true)
                .Index(t => t.Subject_Id)
                .Index(t => t.Lecturer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubjectLecturers", "Lecturer_Id", "dbo.Lecturers");
            DropForeignKey("dbo.SubjectLecturers", "Subject_Id", "dbo.Subjects");
            DropIndex("dbo.SubjectLecturers", new[] { "Lecturer_Id" });
            DropIndex("dbo.SubjectLecturers", new[] { "Subject_Id" });
            DropTable("dbo.SubjectLecturers");
            DropTable("dbo.Subjects");
        }
    }
}
