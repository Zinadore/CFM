using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFM.Data.Models;

namespace CFM.Data
{
    public class CfmDbContext: DbContext
    {
        public CfmDbContext()
            :base("name=cfmConnection")
        {
                Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CfmDbContext>());
        }

        public DbSet<Professor> Professors { get; set; }
        public DbSet<Unit> Units { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Unit>()
                .HasMany(u => u.Teachers)
                .WithMany();    
        }
    } 
}
