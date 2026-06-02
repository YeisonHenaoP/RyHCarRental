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
            // Solo ejecutar si no hay tipos de vehículo (BD vacía)
            if (await context.VehicleTypes.AnyAsync())
                return;

            // ═══ 1. TIPOS DE VEHÍCULO ═══
            var vehicleTypes = new List<VehicleType>
            {
                new() { Name = "Económico",     Description = "Vehículos compactos de bajo consumo, ideales para ciudad" },
                new() { Name = "Sedán",         Description = "Automóvil de gama media, cómodo para viajes largos"       },
                new() { Name = "SUV",           Description = "Vehículo utilitario deportivo, terreno mixto"             },
                new() { Name = "Pickup",        Description = "Camioneta de carga ligera y doble cabina"                 },
                new() { Name = "Lujo",          Description = "Vehículos premium de alta gama"                           },
                new() { Name = "Van / Minivan", Description = "Para grupos o familias, alta capacidad de pasajeros"      },
            };

            context.VehicleTypes.AddRange(vehicleTypes);
            await context.SaveChangesAsync();

            // ═══ 2. SUCURSALES ═══
            var branches = new List<Branch>
            {
                new() { Name = "Sucursal Medellín Centro",    Address = "Cra 50 #45-20",        City = "Medellín",       Phone = "6041234567" },
                new() { Name = "Sucursal Medellín Aeropuerto",Address = "Av. El Dorado #92-30", City = "Medellín",       Phone = "6042345678" },
                new() { Name = "Sucursal Bogotá Norte",       Address = "Calle 127 #15-40",      City = "Bogotá",         Phone = "6017654321" },
                new() { Name = "Sucursal Bogotá El Dorado",   Address = "Av. El Dorado #103-33", City = "Bogotá",         Phone = "6013456789" },
                new() { Name = "Sucursal Cali Centro",        Address = "Calle 5 #38-25",        City = "Cali",           Phone = "6024567890" },
                new() { Name = "Sucursal Barranquilla",       Address = "Cra 53 #72-35",         City = "Barranquilla",   Phone = "6055678901" },
            };

            context.Branches.AddRange(branches);
            await context.SaveChangesAsync();

            // ═══ 3. VEHÍCULOS (3 por tipo = 18 total) ═══
            var vehicles = new List<Vehicle>
            {
                // Económico (vehicleTypes[0])
                new() { Model = "Chevrolet Spark",   Plate = "ECO-001", Year = 2023, DailyRate = 80_000m,  Status = VehicleStatus.Available,     VehicleTypeId = vehicleTypes[0].Id, BranchId = branches[0].Id },
                new() { Model = "Renault Kwid",      Plate = "ECO-002", Year = 2022, DailyRate = 75_000m,  Status = VehicleStatus.Available,     VehicleTypeId = vehicleTypes[0].Id, BranchId = branches[2].Id },
                new() { Model = "Kia Picanto",       Plate = "ECO-003", Year = 2023, DailyRate = 78_000m,  Status = VehicleStatus.Rented,        VehicleTypeId = vehicleTypes[0].Id, BranchId = branches[4].Id },

                // Sedán (vehicleTypes[1])
                new() { Model = "Toyota Corolla",    Plate = "SED-010", Year = 2023, DailyRate = 110_000m, Status = VehicleStatus.Available,     VehicleTypeId = vehicleTypes[1].Id, BranchId = branches[0].Id },
                new() { Model = "Chevrolet Onix",    Plate = "SED-011", Year = 2024, DailyRate = 105_000m, Status = VehicleStatus.Available,     VehicleTypeId = vehicleTypes[1].Id, BranchId = branches[1].Id },
                new() { Model = "Renault Logan",     Plate = "SED-012", Year = 2022, DailyRate = 98_000m,  Status = VehicleStatus.InMaintenance, VehicleTypeId = vehicleTypes[1].Id, BranchId = branches[3].Id },

                // SUV (vehicleTypes[2])
                new() { Model = "Chevrolet Tracker", Plate = "SUV-020", Year = 2023, DailyRate = 170_000m, Status = VehicleStatus.Available,     VehicleTypeId = vehicleTypes[2].Id, BranchId = branches[0].Id },
                new() { Model = "Kia Sportage",      Plate = "SUV-021", Year = 2024, DailyRate = 185_000m, Status = VehicleStatus.Available,     VehicleTypeId = vehicleTypes[2].Id, BranchId = branches[2].Id },
                new() { Model = "Hyundai Tucson",    Plate = "SUV-022", Year = 2023, DailyRate = 180_000m, Status = VehicleStatus.Rented,        VehicleTypeId = vehicleTypes[2].Id, BranchId = branches[5].Id },

                // Pickup (vehicleTypes[3])
                new() { Model = "Toyota Hilux",      Plate = "PIC-030", Year = 2023, DailyRate = 215_000m, Status = VehicleStatus.Available,     VehicleTypeId = vehicleTypes[3].Id, BranchId = branches[1].Id },
                new() { Model = "Ford Ranger",       Plate = "PIC-031", Year = 2022, DailyRate = 205_000m, Status = VehicleStatus.Available,     VehicleTypeId = vehicleTypes[3].Id, BranchId = branches[4].Id },
                new() { Model = "Chevrolet D-Max",   Plate = "PIC-032", Year = 2023, DailyRate = 210_000m, Status = VehicleStatus.InMaintenance, VehicleTypeId = vehicleTypes[3].Id, BranchId = branches[5].Id },

                // Lujo (vehicleTypes[4])
                new() { Model = "BMW Serie 3",       Plate = "LUJ-040", Year = 2024, DailyRate = 380_000m, Status = VehicleStatus.Available,     VehicleTypeId = vehicleTypes[4].Id, BranchId = branches[2].Id },
                new() { Model = "Mercedes Clase C",  Plate = "LUJ-041", Year = 2024, DailyRate = 420_000m, Status = VehicleStatus.Available,     VehicleTypeId = vehicleTypes[4].Id, BranchId = branches[3].Id },

                // Van / Minivan (vehicleTypes[5])
                new() { Model = "Kia Carnival",      Plate = "VAN-050", Year = 2023, DailyRate = 270_000m, Status = VehicleStatus.Available,     VehicleTypeId = vehicleTypes[5].Id, BranchId = branches[0].Id },
                new() { Model = "Hyundai H-1",       Plate = "VAN-051", Year = 2022, DailyRate = 250_000m, Status = VehicleStatus.Available,     VehicleTypeId = vehicleTypes[5].Id, BranchId = branches[5].Id },
            };

            context.Vehicles.AddRange(vehicles);
            await context.SaveChangesAsync();

            // ═══ 4. CLIENTES ═══
            var customers = new List<Customer>
            {
                new() { FullName = "Ana María López",     DocumentId = "1020304050", Email = "ana.lopez@email.com",     Phone = "3001234567" },
                new() { FullName = "Carlos Ruiz Pérez",   DocumentId = "8090706050", Email = "carlos.ruiz@email.com",   Phone = "3109876543" },
                new() { FullName = "Valentina Gómez",     DocumentId = "1033445566", Email = "valentina.gomez@email.com",Phone = "3153456789" },
                new() { FullName = "Andrés Torres Mesa",  DocumentId = "7788990011", Email = "andres.torres@email.com", Phone = "3204567890" },
                new() { FullName = "Daniela Martínez",    DocumentId = "2233445566", Email = "daniela.martinez@email.com",Phone = "3005678901"},
                new() { FullName = "Miguel Herrera Díaz", DocumentId = "9988776655", Email = "miguel.herrera@email.com", Phone = "3116789012" },
            };

            context.Customers.AddRange(customers);
            await context.SaveChangesAsync();

            // ═══ 5. ARRIENDOS CON DETALLE ═══

            // Arriendo 1: Ana arrienda el Corolla (Completado)
            var rental1 = new Rental
            {
                CustomerId = customers[0].Id,
                StartDate = new DateTime(2026, 5, 1),
                EndDate = new DateTime(2026, 5, 5),
                TotalCost = vehicles[3].DailyRate * 4,   // 4 días × Corolla
                Status = RentalStatus.Completed,
            };
            context.Rentals.Add(rental1);
            await context.SaveChangesAsync();

            context.RentalDetails.Add(new RentalDetail
            {
                RentalId = rental1.Id,
                VehicleId = vehicles[3].Id,               // Toyota Corolla
                SubTotal = vehicles[3].DailyRate * 4,
            });
            await context.SaveChangesAsync();

            // Arriendo 2: Carlos arrienda el Tracker SUV (Activo)
            var rental2 = new Rental
            {
                CustomerId = customers[1].Id,
                StartDate = new DateTime(2026, 6, 1),
                EndDate = new DateTime(2026, 6, 6),
                TotalCost = vehicles[6].DailyRate * 5,   // 5 días × Tracker
                Status = RentalStatus.Active,
            };
            context.Rentals.Add(rental2);
            await context.SaveChangesAsync();

            context.RentalDetails.Add(new RentalDetail
            {
                RentalId = rental2.Id,
                VehicleId = vehicles[6].Id,               // Chevrolet Tracker
                SubTotal = vehicles[6].DailyRate * 5,
            });
            await context.SaveChangesAsync();

            // Arriendo 3: Valentina arrienda la Kia Picanto (Cancelado)
            var rental3 = new Rental
            {
                CustomerId = customers[2].Id,
                StartDate = new DateTime(2026, 5, 20),
                EndDate = new DateTime(2026, 5, 23),
                TotalCost = vehicles[2].DailyRate * 3,
                Status = RentalStatus.Cancelled,
            };
            context.Rentals.Add(rental3);
            await context.SaveChangesAsync();

            context.RentalDetails.Add(new RentalDetail
            {
                RentalId = rental3.Id,
                VehicleId = vehicles[2].Id,               // Kia Picanto
                SubTotal = vehicles[2].DailyRate * 3,
            });
            await context.SaveChangesAsync();
        }
    }
}
