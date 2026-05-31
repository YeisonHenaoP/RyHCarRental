using RyHCarRental.Domain.Entities;
using RyHCarRental.Domain.Interfaces.Repositories;
using RyHCarRental.Domain.Interfaces.Services;

namespace RyHCarRental.Domain.Services
{
    public class VehicleTypeService : IVehicleTypeService
    {
        private readonly IVehicleTypeRepository _vehicleTypeRepository;

        public VehicleTypeService(IVehicleTypeRepository vehicleTypeRepository)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
        }

        public async Task<IEnumerable<VehicleType>> GetAllAsync()
            => await _vehicleTypeRepository.GetAllAsync();

        public async Task<VehicleType?> GetByIdAsync(int id)
            => await _vehicleTypeRepository.GetByIdAsync(id);

        public async Task<VehicleType> CreateAsync(VehicleType vehicleType)
        {
            if (string.IsNullOrWhiteSpace(vehicleType.Name))
                throw new InvalidOperationException("El nombre del tipo de vehículo es obligatorio.");

            var existing = await _vehicleTypeRepository.GetByNameAsync(vehicleType.Name);
            if (existing != null)
                throw new InvalidOperationException($"Ya existe un tipo de vehículo con el nombre '{vehicleType.Name}'.");

            await _vehicleTypeRepository.AddAsync(vehicleType);
            await _vehicleTypeRepository.SaveChangesAsync();
            return vehicleType;
        }

        public async Task UpdateAsync(int id, VehicleType vehicleType)
        {
            var existing = await _vehicleTypeRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Tipo de vehículo con ID {id} no encontrado.");

            if (string.IsNullOrWhiteSpace(vehicleType.Name))
                throw new InvalidOperationException("El nombre del tipo de vehículo es obligatorio.");

            var duplicate = await _vehicleTypeRepository.GetByNameAsync(vehicleType.Name);
            if (duplicate != null && duplicate.Id != id)
                throw new InvalidOperationException($"Ya existe un tipo de vehículo con el nombre '{vehicleType.Name}'.");

            existing.Name = vehicleType.Name;
            existing.Description = vehicleType.Description;
            _vehicleTypeRepository.Update(existing);
            await _vehicleTypeRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _vehicleTypeRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Tipo de vehículo con ID {id} no encontrado.");

            _vehicleTypeRepository.Delete(existing);
            await _vehicleTypeRepository.SaveChangesAsync();
        }
    }
}
