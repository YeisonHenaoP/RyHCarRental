using Microsoft.EntityFrameworkCore;
using RyHCarRental.DataAccess.Context;
using RyHCarRental.Domain.Entities;
using RyHCarRental.Domain.Interfaces.Repositories;

namespace RyHCarRental.DataAccess.Repositories
{
    public class VehicleTypeRepository : GenericRepository<VehicleType>, IVehicleTypeRepository
    {
        public VehicleTypeRepository(ApplicationDbContext context) : base(context) { }

        public async Task<VehicleType?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(vt => vt.Name == name);
        }
    }
}
