using System.Collections.Generic;
using CFM.Data.Models;

namespace CFM.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CFM.Data.CfmDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "CFM.Data.CfmDbContext";
        }

        protected override void Seed(CFM.Data.CfmDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            Professor[] profs = new Professor[4]
            {
                new Professor {FirstName = "Kwstas", LastName = "Tzatzikis"},
                new Professor {FirstName = "Kurios", LastName = "Maimoudis"},
                new Professor {FirstName = "Tyro", LastName = "Brwmikoulas"},
                new Professor {FirstName = "I", LastName = "Katsika"}
            };

            context.Professors.AddOrUpdate(
                p => p.Id,
                profs
            );

            Unit[] units = new Unit[2]
            {
                new Unit {Code = "CCP123", Title = "My First Unit", Teachers = new List<Professor> { profs[1], profs[3]} },
                new Unit {Code = "CCP456", Title = "My Second Unit", Teachers = new List<Professor>{ profs[0], profs[2]} }
            };

            context.Units.AddOrUpdate(
                u => u.Id,
                units
            );

            Assignment[] assignments = new Assignment[1]
            {
                new Assignment {Title = "That One Assignment", Type = "Report", Deadline = DateTime.Now, Unit = units[0]}
            };

            context.Assignments.AddOrUpdate(
                a => a.Id,
                assignments
            );
        }
    }
}
