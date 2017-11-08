namespace CookBook_WPF.Migrations
{
    using CookBook_WPF.Data;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CookBook_WPF.Data.CookBookModel>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "CookBook_WPF.Data.CookBookModel";
        }

        protected override void Seed(CookBook_WPF.Data.CookBookModel context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Measures.AddOrUpdate(
              p => p.szMeasureName,
              new Measure
              {
                  szMeasureName = "דנ.",
                  bIsDefault = true
              },
               new Measure
               {
                   szMeasureName = "לכ.",
                   bIsDefault = false
               }, new Measure
               {
                   szMeasureName = "ק.כ.",
                   bIsDefault = false
               }, new Measure
               {
                   szMeasureName = "סע.כ.",
                   bIsDefault = false
               },
                new Measure
                {
                    szMeasureName = "רע.",
                    bIsDefault = false
                }
            );

        }
    }
}
