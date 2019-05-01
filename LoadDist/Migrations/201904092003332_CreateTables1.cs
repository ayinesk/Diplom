namespace LoadDist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTables1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupNumber = c.String(),
                        StudentsCount = c.Int(nullable: false),
                        Syllabus_Id = c.Int(),
                        Stream_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Syllabus", t => t.Syllabus_Id)
                .ForeignKey("dbo.Streams", t => t.Stream_Id)
                .Index(t => t.Syllabus_Id)
                .Index(t => t.Stream_Id);
            
            CreateTable(
                "dbo.Syllabus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AdmissionYear = c.Int(nullable: false),
                        Specialty_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Specialties", t => t.Specialty_Id)
                .Index(t => t.Specialty_Id);
            
            CreateTable(
                "dbo.Specialties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SpecialtyCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Streams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SyllabusContents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HasCourseWork = c.Boolean(nullable: false),
                        LectureHours = c.Int(nullable: false),
                        LabsHours = c.Int(nullable: false),
                        PracticalHours = c.Int(nullable: false),
                        HasExam = c.Boolean(nullable: false),
                        HasTest = c.Boolean(nullable: false),
                        CourseWorkHours = c.Int(nullable: false),
                        Subject_Id = c.Int(),
                        Syllabus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjects", t => t.Subject_Id)
                .ForeignKey("dbo.Syllabus", t => t.Syllabus_Id)
                .Index(t => t.Subject_Id)
                .Index(t => t.Syllabus_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SyllabusContents", "Syllabus_Id", "dbo.Syllabus");
            DropForeignKey("dbo.SyllabusContents", "Subject_Id", "dbo.Subjects");
            DropForeignKey("dbo.Groups", "Stream_Id", "dbo.Streams");
            DropForeignKey("dbo.Groups", "Syllabus_Id", "dbo.Syllabus");
            DropForeignKey("dbo.Syllabus", "Specialty_Id", "dbo.Specialties");
            DropIndex("dbo.SyllabusContents", new[] { "Syllabus_Id" });
            DropIndex("dbo.SyllabusContents", new[] { "Subject_Id" });
            DropIndex("dbo.Syllabus", new[] { "Specialty_Id" });
            DropIndex("dbo.Groups", new[] { "Stream_Id" });
            DropIndex("dbo.Groups", new[] { "Syllabus_Id" });
            DropTable("dbo.SyllabusContents");
            DropTable("dbo.Streams");
            DropTable("dbo.Specialties");
            DropTable("dbo.Syllabus");
            DropTable("dbo.Groups");
        }
    }
}
