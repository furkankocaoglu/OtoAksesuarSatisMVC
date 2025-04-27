namespace OtoAksesuarSatisWebAp.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using OtoAksesuarSatisWebAp.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<OtoAksesuarSatisDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(OtoAksesuarSatisDB context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

        }
    }
}
