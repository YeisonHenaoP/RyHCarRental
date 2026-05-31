using RyHCarRental.Domain.Entities;

namespace RyHCarRental.Domain.Interfaces.Repositories
{
    public interface IVehicleTypeRepository : IGenericRepository<VehicleType>
    {
        Task<VehicleType?> GetByNameAsync(string name);
    }
}
