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
            public DbSet<OrderDetails> OrderDetails { get; set; }

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


            modelBuilder.Entity<Order>()
            .HasOne(o => o.Authorize)               // An order has one customer
            .WithMany(c => c.Orders)                // A customer can have many orders
            .HasForeignKey(o => o.CustomerId); ;      // CustomerId in Order is the foreign key

            // Configure the foreign key relationship between OrderDetail and Product
            modelBuilder.Entity<OrderDetails>()
                .HasOne(od => od.Product)               // An order detail has one product
                .WithMany()                             // A product can appear in many order details
                .HasForeignKey(od => od.ProductId);    // ProductId in OrderDetail is the foreign key

            // Configure the foreign key relationship between OrderDetail and Order
            modelBuilder.Entity<Order>()
               .HasOne(o => o.Trail)
               .WithMany(t => t.Orders)
               .HasForeignKey(o => o.TrailId)
               .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Trail)
                .WithMany(t => t.Products)
                .HasForeignKey(p => p.TrailId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete

            modelBuilder.Entity<OrderDetails>()
                .HasOne(od => od.Trail)
                .WithMany(t => t.OrderDetails)
                .HasForeignKey(od => od.TrailId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete

            modelBuilder.Entity<Category>()
                .HasOne(c => c.Trail)
                .WithMany(t => t.Categories)
                .HasForeignKey(c => c.TrailId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete

        }
     }
}
