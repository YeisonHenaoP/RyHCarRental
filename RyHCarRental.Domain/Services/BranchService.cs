using RyHCarRental.Domain.Entities;
using RyHCarRental.Domain.Interfaces.Repositories;
using RyHCarRental.Domain.Interfaces.Services;

namespace RyHCarRental.Domain.Services
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _branchRepository;

        public BranchService(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<IEnumerable<Branch>> GetAllAsync()
            => await _branchRepository.GetAllAsync();

        public async Task<Branch?> GetByIdAsync(int id)
            => await _branchRepository.GetByIdAsync(id);

        public async Task<Branch> CreateAsync(Branch branch)
        {
            if (string.IsNullOrWhiteSpace(branch.Name))
                throw new InvalidOperationException("El nombre de la sucursal es obligatorio.");
            if (string.IsNullOrWhiteSpace(branch.City))
                throw new InvalidOperationException("La ciudad es obligatoria.");

            var existing = await _branchRepository.GetByNameAsync(branch.Name);
            if (existing != null)
                throw new InvalidOperationException($"Ya existe una sucursal con el nombre '{branch.Name}'.");

            await _branchRepository.AddAsync(branch);
            await _branchRepository.SaveChangesAsync();
            return branch;
        }

        public async Task UpdateAsync(int id, Branch branch)
        {
            var existing = await _branchRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Sucursal con ID {id} no encontrada.");

            if (string.IsNullOrWhiteSpace(branch.Name))
                throw new InvalidOperationException("El nombre de la sucursal es obligatorio.");

            var duplicate = await _branchRepository.GetByNameAsync(branch.Name);
            if (duplicate != null && duplicate.Id != id)
                throw new InvalidOperationException($"Ya existe una sucursal con el nombre '{branch.Name}'.");

            existing.Name = branch.Name;
            existing.Address = branch.Address;
            existing.City = branch.City;
            existing.Phone = branch.Phone;
            _branchRepository.Update(existing);
            await _branchRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _branchRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Sucursal con ID {id} no encontrada.");

            _branchRepository.Delete(existing);
            await _branchRepository.SaveChangesAsync();
        }
    }
}
