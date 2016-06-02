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
            Database.SetInitializer(new CreateDatabaseIfNotExists<CfmDbContext>());
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Professor> Professors { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Goal> Goals { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Unit>()
                .HasMany(u => u.Teachers)
                .WithMany();
            modelBuilder.Entity<Unit>()
                .HasMany(u => u.Assignments)
                .WithOptional(a => a.Unit);
            modelBuilder.Entity<Assignment>()
                .HasMany(a => a.Feedbacks)
                .WithRequired(f => f.Assignment);
            modelBuilder.Entity<Assignment>()
                .HasMany(a => a.Goals)
                .WithRequired(g => g.Assignment);
        }
    } 
}
