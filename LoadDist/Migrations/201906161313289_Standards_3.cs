namespace LoadDist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Standards_3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Standards", "ExamStandard", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Standards", "TestStandard", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Standards", "ConsultationStandard", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Standards", "ConsultationStandard", c => c.Single(nullable: false));
            AlterColumn("dbo.Standards", "TestStandard", c => c.Single(nullable: false));
            AlterColumn("dbo.Standards", "ExamStandard", c => c.Single(nullable: false));
        }
    }
}
