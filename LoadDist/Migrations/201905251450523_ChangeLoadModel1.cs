namespace LoadDist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeLoadModel1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Loads", "Consultation", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Loads", "Consultation");
        }
    }
}
