using Microsoft.EntityFrameworkCore;
using RyHCarRental.DataAccess.Context;
using RyHCarRental.Domain.Entities;
using RyHCarRental.Domain.Interfaces.Repositories;

namespace RyHCarRental.DataAccess.Repositories
{
    public class BranchRepository : GenericRepository<Branch>, IBranchRepository
    {
        public BranchRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Branch?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(b => b.Name == name);
        }
    }
}
