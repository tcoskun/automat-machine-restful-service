using System;
using System.Linq;
using AutomatMachine.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace AutomatMachine.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        { }

        public DbSet<Product> Product { get; set; }
        public DbSet<Process> Process { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Process>()
                .HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(s => s.ProductId);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Entity && (
                                e.State == EntityState.Added
                                || e.State == EntityState.Modified));

            var dateTimeNow = DateTime.Now;
            foreach (var entityEntry in entries)
            {
                ((Entity)entityEntry.Entity).ModifyDate = dateTimeNow;

                if (entityEntry.State != EntityState.Added) continue;

                ((Entity) entityEntry.Entity).Id = Guid.NewGuid();
                ((Entity)entityEntry.Entity).CreateDate = dateTimeNow;
            }

            return base.SaveChanges();
        }
    }
}
