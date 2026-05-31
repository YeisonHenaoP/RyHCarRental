using Microsoft.EntityFrameworkCore;
using RyHCarRental.DataAccess.Context;
using RyHCarRental.Domain.Entities;
using RyHCarRental.Domain.Enums;

namespace RyHCarRental.DataAccess.Seeders
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.VehicleTypes.AnyAsync())
                return;

            var vehicleTypes = new List<VehicleType>
            {
                new() { Name = "Sedán", Description = "Automóvil compacto o mediano" },
                new() { Name = "SUV", Description = "Vehículo utilitario deportivo" },
                new() { Name = "Pickup", Description = "Camioneta de carga ligera" }
            };
            context.VehicleTypes.AddRange(vehicleTypes);
            await context.SaveChangesAsync();

            var branches = new List<Branch>
            {
                new() { Name = "Sucursal Medellín Centro", Address = "Cra 50 #45-20", City = "Medellín", Phone = "6041234567" },
                new() { Name = "Sucursal Bogotá Norte", Address = "Calle 127 #15-40", City = "Bogotá", Phone = "6017654321" }
            };
            context.Branches.AddRange(branches);
            await context.SaveChangesAsync();

            var vehicles = new List<Vehicle>
            {
                new() { Model = "Toyota Corolla", Plate = "ABC123", Year = 2023, DailyRate = 85000, Status = VehicleStatus.Available, VehicleTypeId = vehicleTypes[0].Id, BranchId = branches[0].Id },
                new() { Model = "Mazda CX-5", Plate = "DEF456", Year = 2024, DailyRate = 120000, Status = VehicleStatus.Available, VehicleTypeId = vehicleTypes[1].Id, BranchId = branches[0].Id },
                new() { Model = "Chevrolet D-Max", Plate = "GHI789", Year = 2022, DailyRate = 150000, Status = VehicleStatus.Available, VehicleTypeId = vehicleTypes[2].Id, BranchId = branches[1].Id }
            };
            context.Vehicles.AddRange(vehicles);
            await context.SaveChangesAsync();

            var customers = new List<Customer>
            {
                new() { FullName = "Ana María López", DocumentId = "1020304050", Email = "ana.lopez@email.com", Phone = "3001234567" },
                new() { FullName = "Carlos Ruiz", DocumentId = "8090706050", Email = "carlos.ruiz@email.com", Phone = "3109876543" }
            };
            context.Customers.AddRange(customers);
            await context.SaveChangesAsync();
        }
    }
}
