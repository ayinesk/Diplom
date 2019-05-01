namespace LoadDist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTablesAndRows : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SubjectLecturers", newName: "LecturerSubjects");
            DropPrimaryKey("dbo.LecturerSubjects");
            CreateTable(
                "dbo.CourseWorks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseWorkHours = c.Int(nullable: false),
                        Subject_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjects", t => t.Subject_Id)
                .Index(t => t.Subject_Id);
            
            CreateTable(
                "dbo.Loads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StreamsCount = c.Int(nullable: false),
                        SubgroupsCount = c.Int(nullable: false),
                        Lecturer_Id = c.Int(),
                        Subject_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lecturers", t => t.Lecturer_Id)
                .ForeignKey("dbo.Subjects", t => t.Subject_Id)
                .Index(t => t.Lecturer_Id)
                .Index(t => t.Subject_Id);
            
            CreateTable(
                "dbo.Standards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StandardName = c.String(),
                        StandardValue = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Groups", "SubgroupsCount", c => c.Int(nullable: false));
            AddColumn("dbo.SyllabusContents", "ExamHours", c => c.Int(nullable: false));
            AddColumn("dbo.SyllabusContents", "TestHours", c => c.Int(nullable: false));
            AddColumn("dbo.SyllabusContents", "Term", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.LecturerSubjects", new[] { "Lecturer_Id", "Subject_Id" });
            DropColumn("dbo.SyllabusContents", "HasCourseWork");
            DropColumn("dbo.SyllabusContents", "HasExam");
            DropColumn("dbo.SyllabusContents", "HasTest");
            DropColumn("dbo.SyllabusContents", "CourseWorkHours");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SyllabusContents", "CourseWorkHours", c => c.Int(nullable: false));
            AddColumn("dbo.SyllabusContents", "HasTest", c => c.Boolean(nullable: false));
            AddColumn("dbo.SyllabusContents", "HasExam", c => c.Boolean(nullable: false));
            AddColumn("dbo.SyllabusContents", "HasCourseWork", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Loads", "Subject_Id", "dbo.Subjects");
            DropForeignKey("dbo.Loads", "Lecturer_Id", "dbo.Lecturers");
            DropForeignKey("dbo.CourseWorks", "Subject_Id", "dbo.Subjects");
            DropIndex("dbo.Loads", new[] { "Subject_Id" });
            DropIndex("dbo.Loads", new[] { "Lecturer_Id" });
            DropIndex("dbo.CourseWorks", new[] { "Subject_Id" });
            DropPrimaryKey("dbo.LecturerSubjects");
            DropColumn("dbo.SyllabusContents", "Term");
            DropColumn("dbo.SyllabusContents", "TestHours");
            DropColumn("dbo.SyllabusContents", "ExamHours");
            DropColumn("dbo.Groups", "SubgroupsCount");
            DropTable("dbo.Standards");
            DropTable("dbo.Loads");
            DropTable("dbo.CourseWorks");
            AddPrimaryKey("dbo.LecturerSubjects", new[] { "Subject_Id", "Lecturer_Id" });
            RenameTable(name: "dbo.LecturerSubjects", newName: "SubjectLecturers");
        }
    }
}
