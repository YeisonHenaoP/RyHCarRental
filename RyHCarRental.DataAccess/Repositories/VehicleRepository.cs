using Microsoft.EntityFrameworkCore;
using RyHCarRental.DataAccess.Context;
using RyHCarRental.Domain.Entities;
using RyHCarRental.Domain.Interfaces.Repositories;

namespace RyHCarRental.DataAccess.Repositories
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Vehicle?> GetByPlateAsync(string plate)
        {
            return await _dbSet
                .FirstOrDefaultAsync(v => v.Plate == plate);
        }

        public async Task<IEnumerable<Vehicle>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(v => v.VehicleType)
                .Include(v => v.Branch)
                .ToListAsync();
        }

        public async Task<Vehicle?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(v => v.VehicleType)
                .Include(v => v.Branch)
                .FirstOrDefaultAsync(v => v.Id == id);
        }
    }
}
