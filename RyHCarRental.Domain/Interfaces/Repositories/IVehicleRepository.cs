using RyHCarRental.Domain.Entities;

namespace RyHCarRental.Domain.Interfaces.Repositories
{
    public interface IVehicleRepository : IGenericRepository<Vehicle>
    {
        Task<Vehicle?> GetByPlateAsync(string plate);
        Task<IEnumerable<Vehicle>> GetAllWithDetailsAsync();
        Task<Vehicle?> GetByIdWithDetailsAsync(int id);
    }
}
