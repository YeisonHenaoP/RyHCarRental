using RyHCarRental.Domain.Entities;

namespace RyHCarRental.Domain.Interfaces
{
    public interface IRentalRepository : IGenericRepository<Rental>
    {
        Task<IEnumerable<Rental>> GetActiveRentalsByCustomerAsync(int customerId);
        Task<bool> IsVehicleAvailableAsync(int vehicleId, DateTime start, DateTime end);
    }
}