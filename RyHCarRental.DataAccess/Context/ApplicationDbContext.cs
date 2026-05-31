using Microsoft.EntityFrameworkCore;
using RyHCarRental.Domain.Entities;

namespace RyHCarRental.DataAccess.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<RentalDetail> RentalDetails { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<Branch> Branches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Rentals)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RentalDetail>()
                .HasOne(rd => rd.Rental)
                .WithMany(r => r.RentalDetails)
                .HasForeignKey(rd => rd.RentalId);

            modelBuilder.Entity<RentalDetail>()
                .HasOne(rd => rd.Vehicle)
                .WithMany()
                .HasForeignKey(rd => rd.VehicleId);

            modelBuilder.Entity<Vehicle>()
                .ToTable("Vehicle");

            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.VehicleType)
                .WithMany(vt => vt.Vehicles)
                .HasForeignKey(v => v.VehicleTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Branch)
                .WithMany(b => b.Vehicles)
                .HasForeignKey(v => v.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VehicleType>()
                .HasIndex(vt => vt.Name)
                .IsUnique();

            modelBuilder.Entity<Branch>()
                .HasIndex(b => b.Name)
                .IsUnique();

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
