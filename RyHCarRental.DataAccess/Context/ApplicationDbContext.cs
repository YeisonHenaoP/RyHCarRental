using Microsoft.EntityFrameworkCore;
using RyHCarRental.Domain.Entities;

namespace RyHCarRental.DataAccess.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSets de tus entidades
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<RentalDetail> RentalDetails { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación Customer 1:N Rental
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Rentals)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación N:M mediante RentalDetail
            modelBuilder.Entity<RentalDetail>()
                .HasOne(rd => rd.Rental)
                .WithMany(r => r.RentalDetails)
                .HasForeignKey(rd => rd.RentalId);

            modelBuilder.Entity<RentalDetail>()
                .HasOne(rd => rd.Vehicle)
                .WithMany() 
                .HasForeignKey(rd => rd.VehicleId);

            // Restricciones e índices
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.DocumentId)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Rental>()
                .Property(r => r.TotalCost)
                .HasPrecision(18, 2);
        }
    }
}
