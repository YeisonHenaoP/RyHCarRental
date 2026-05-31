using RyHCarRental.Domain.Entities;

namespace RyHCarRental.Domain.Interfaces.Services
{
    public interface IVehicleTypeService
    {
        Task<IEnumerable<VehicleType>> GetAllAsync();
        Task<VehicleType?> GetByIdAsync(int id);
        Task<VehicleType> CreateAsync(VehicleType vehicleType);
        Task UpdateAsync(int id, VehicleType vehicleType);
        Task DeleteAsync(int id);
    }
}
