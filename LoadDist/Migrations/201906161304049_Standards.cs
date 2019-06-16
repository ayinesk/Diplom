namespace LoadDist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Standards : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Standards", "ExamStandard", c => c.Int(nullable: false));
            AddColumn("dbo.Standards", "TestStandard", c => c.Int(nullable: false));
            AddColumn("dbo.Standards", "ConsultationStandard", c => c.Int(nullable: false));
            DropColumn("dbo.Standards", "StandardName");
            DropColumn("dbo.Standards", "StandardValue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Standards", "StandardValue", c => c.Int(nullable: false));
            AddColumn("dbo.Standards", "StandardName", c => c.String());
            DropColumn("dbo.Standards", "ConsultationStandard");
            DropColumn("dbo.Standards", "TestStandard");
            DropColumn("dbo.Standards", "ExamStandard");
        }
    }
}
