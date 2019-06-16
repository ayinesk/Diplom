namespace LoadDist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSyllabusContent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SyllabusContents", "Consultation", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SyllabusContents", "Consultation");
        }
    }
}
