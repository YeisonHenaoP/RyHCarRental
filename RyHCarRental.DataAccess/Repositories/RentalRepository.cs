using Microsoft.EntityFrameworkCore;
using RyHCarRental.DataAccess.Context;
using RyHCarRental.Domain.Entities;
using RyHCarRental.Domain.Enums;
using RyHCarRental.Domain.Interfaces;

namespace RyHCarRental.DataAccess.Repositories
{
    public class RentalRepository : GenericRepository<Rental>, IRentalRepository
    {
        public RentalRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Rental>> GetActiveRentalsByCustomerAsync(int customerId)
        {
            return await _dbSet
                .Where(r => r.CustomerId == customerId && r.Status == RentalStatus.Active)
                .Include(r => r.RentalDetails)
                .ThenInclude(rd => rd.Vehicle)
                .ToListAsync();
        }

        public async Task<bool> IsVehicleAvailableAsync(int vehicleId, DateTime start, DateTime end)
        {
            // Verifica si el vehículo está en algún alquiler activo que solape las fechas
            return !await _context.Set<RentalDetail>()
                .AnyAsync(rd => rd.VehicleId == vehicleId &&
                                rd.Rental.Status == RentalStatus.Active &&
                                rd.Rental.StartDate < end &&
                                rd.Rental.EndDate > start);
        }
    }
}