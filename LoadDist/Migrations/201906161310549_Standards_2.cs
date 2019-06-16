namespace LoadDist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Standards_2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Standards", "ExamStandard", c => c.Single(nullable: false));
            AlterColumn("dbo.Standards", "TestStandard", c => c.Single(nullable: false));
            AlterColumn("dbo.Standards", "ConsultationStandard", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Standards", "ConsultationStandard", c => c.Int(nullable: false));
            AlterColumn("dbo.Standards", "TestStandard", c => c.Int(nullable: false));
            AlterColumn("dbo.Standards", "ExamStandard", c => c.Int(nullable: false));
        }
    }
}
