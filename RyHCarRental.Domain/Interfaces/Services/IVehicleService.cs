using RyHCarRental.Domain.Entities;

namespace RyHCarRental.Domain.Interfaces.Services
{
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle?> GetByIdAsync(int id);
        Task<Vehicle> CreateAsync(Vehicle vehicle);
        Task UpdateAsync(int id, Vehicle vehicle);
        Task DeleteAsync(int id);
    }
}
