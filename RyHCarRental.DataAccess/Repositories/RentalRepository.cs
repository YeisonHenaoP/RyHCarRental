using Microsoft.EntityFrameworkCore;
using RyHCarRental.DataAccess.Context;
using RyHCarRental.Domain.Entities;
using RyHCarRental.Domain.Enums;
using RyHCarRental.Domain.Interfaces.Repositories;

namespace RyHCarRental.DataAccess.Repositories
{
    public class RentalRepository : GenericRepository<Rental>, IRentalRepository
    {
        public RentalRepository(ApplicationDbContext context) : base(context) { }

        // ── Sobreescribimos GetAllAsync para incluir Customer y RentalDetails ──
        public new async Task<IEnumerable<Rental>> GetAllAsync()
        {
            return await _dbSet
                .Include(r => r.Customer)
                .Include(r => r.RentalDetails)
                    .ThenInclude(rd => rd.Vehicle)
                .ToListAsync();
        }

        // ── Sobreescribimos GetByIdAsync para incluir las mismas relaciones ──
        public new async Task<Rental?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(r => r.Customer)
                .Include(r => r.RentalDetails)
                    .ThenInclude(rd => rd.Vehicle)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Rental>> GetActiveRentalsByCustomerAsync(int customerId)
        {
            return await _dbSet
                .Where(r => r.CustomerId == customerId && r.Status == RentalStatus.Active)
                .Include(r => r.Customer)
                .Include(r => r.RentalDetails)
                    .ThenInclude(rd => rd.Vehicle)
                .ToListAsync();
        }

        public async Task<bool> IsVehicleAvailableAsync(int vehicleId, DateTime start, DateTime end)
        {
            return !await _context.Set<RentalDetail>()
                .AnyAsync(rd => rd.VehicleId == vehicleId &&
                                rd.Rental.Status == RentalStatus.Active &&
                                rd.Rental.StartDate < end &&
                                rd.Rental.EndDate > start);
        }
    }
}