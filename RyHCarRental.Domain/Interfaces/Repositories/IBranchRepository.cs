using RyHCarRental.Domain.Entities;

namespace RyHCarRental.Domain.Interfaces.Repositories
{
    public interface IBranchRepository : IGenericRepository<Branch>
    {
        Task<Branch?> GetByNameAsync(string name);
    }
}
