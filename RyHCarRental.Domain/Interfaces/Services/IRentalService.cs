using RyHCarRental.Domain.Entities;

namespace RyHCarRental.Domain.Interfaces.Services
{
    public interface IRentalService
    {
        Task<IEnumerable<Rental>> GetAllAsync();
        Task<Rental?> GetByIdAsync(int id);
        Task<Rental> CreateAsync(Rental rental, IEnumerable<int> vehicleIds);
        Task CompleteRentalAsync(int rentalId);
        Task CancelRentalAsync(int rentalId);
    }
}
