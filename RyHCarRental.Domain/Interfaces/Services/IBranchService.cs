using RyHCarRental.Domain.Entities;

namespace RyHCarRental.Domain.Interfaces.Services
{
    public interface IBranchService
    {
        Task<IEnumerable<Branch>> GetAllAsync();
        Task<Branch?> GetByIdAsync(int id);
        Task<Branch> CreateAsync(Branch branch);
        Task UpdateAsync(int id, Branch branch);
        Task DeleteAsync(int id);
    }
}
