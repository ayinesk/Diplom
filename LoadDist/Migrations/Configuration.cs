namespace LoadDist.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LoadDist.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "LoadDist.Models.ApplicationDbContext";
        }

        protected override void Seed(LoadDist.Models.ApplicationDbContext context)
        {
            context.Standards.AddOrUpdate(new Models.DataModels.Standard
            {
                ExamStandard = 0.2M,
                ConsultationStandard = 0.2M,
                TestStandard = 0.2M
            });
        }
    }
}
