using Microsoft.EntityFrameworkCore;
using NurseryMart.Entities;
using System.Data.SqlClient;

namespace NurseryMart.Repositories
{
    public class NurseryMartDbContext : DbContext
    {

        public NurseryMartDbContext(DbContextOptions<NurseryMartDbContext> options) : base(options) { }

            // DbSets represent tables in the SQL database
            public DbSet<Authorize> Authorize { get; set; }
            public DbSet<Category> Category { get; set; }
            public DbSet<Order> Order { get; set; }
            public DbSet<Product> Product { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            base.OnModelCreating(modelBuilder);

            // One-to-many relationship between Authorize and Trail
            modelBuilder.Entity<Authorize>()
                .HasOne(a => a.Trail)  // Authorize has one Trail
                .WithMany(t => t.Authorize)  // Trail can have many Authorizes
                .HasForeignKey(a => a.TrailId)  // Foreign key in Authorize pointing to Trail
                .OnDelete(DeleteBehavior.SetNull);  // Optional: If Trail is deleted, set TrailId to null in Authorize (for cascading behavior)
            modelBuilder.Entity<Authorize>()
               .HasKey(c => c.Id);
            modelBuilder.Entity<Category>()
               .HasKey(c => c.Id);

        }
        }
}
