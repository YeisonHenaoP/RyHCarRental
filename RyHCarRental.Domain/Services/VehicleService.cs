using RyHCarRental.Domain.Entities;
using RyHCarRental.Domain.Enums;
using RyHCarRental.Domain.Interfaces.Repositories;
using RyHCarRental.Domain.Interfaces.Services;

namespace RyHCarRental.Domain.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IVehicleTypeRepository _vehicleTypeRepository;
        private readonly IBranchRepository _branchRepository;

        public VehicleService(
            IVehicleRepository vehicleRepository,
            IVehicleTypeRepository vehicleTypeRepository,
            IBranchRepository branchRepository)
        {
            _vehicleRepository = vehicleRepository;
            _vehicleTypeRepository = vehicleTypeRepository;
            _branchRepository = branchRepository;
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
            => await _vehicleRepository.GetAllWithDetailsAsync();

        public async Task<Vehicle?> GetByIdAsync(int id)
            => await _vehicleRepository.GetByIdWithDetailsAsync(id);

        public async Task<Vehicle> CreateAsync(Vehicle vehicle)
        {
            await ValidateVehicleAsync(vehicle);

            var existingPlate = await _vehicleRepository.GetByPlateAsync(vehicle.Plate);
            if (existingPlate != null)
                throw new InvalidOperationException($"Ya existe un vehículo con la placa '{vehicle.Plate}'.");

            vehicle.Status = VehicleStatus.Available;
            await _vehicleRepository.AddAsync(vehicle);
            await _vehicleRepository.SaveChangesAsync();

            return (await _vehicleRepository.GetByIdWithDetailsAsync(vehicle.Id))!;
        }

        public async Task UpdateAsync(int id, Vehicle vehicle)
        {
            var existing = await _vehicleRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Vehículo con ID {id} no encontrado.");

            if (existing.Status == VehicleStatus.Rented)
                throw new InvalidOperationException("No se puede modificar un vehículo que está alquilado.");

            await ValidateVehicleAsync(vehicle);

            var duplicatePlate = await _vehicleRepository.GetByPlateAsync(vehicle.Plate);
            if (duplicatePlate != null && duplicatePlate.Id != id)
                throw new InvalidOperationException($"Ya existe un vehículo con la placa '{vehicle.Plate}'.");

            existing.Model = vehicle.Model;
            existing.Plate = vehicle.Plate;
            existing.Year = vehicle.Year;
            existing.DailyRate = vehicle.DailyRate;
            existing.Status = vehicle.Status;
            existing.VehicleTypeId = vehicle.VehicleTypeId;
            existing.BranchId = vehicle.BranchId;

            _vehicleRepository.Update(existing);
            await _vehicleRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _vehicleRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Vehículo con ID {id} no encontrado.");

            if (existing.Status == VehicleStatus.Rented)
                throw new InvalidOperationException("No se puede eliminar un vehículo que está alquilado.");

            _vehicleRepository.Delete(existing);
            await _vehicleRepository.SaveChangesAsync();
        }

        private async Task ValidateVehicleAsync(Vehicle vehicle)
        {
            if (string.IsNullOrWhiteSpace(vehicle.Model))
                throw new InvalidOperationException("El modelo es obligatorio.");
            if (string.IsNullOrWhiteSpace(vehicle.Plate))
                throw new InvalidOperationException("La placa es obligatoria.");
            if (vehicle.DailyRate <= 0)
                throw new InvalidOperationException("La tarifa diaria debe ser mayor a cero.");
            if (vehicle.Year < 1990 || vehicle.Year > DateTime.UtcNow.Year + 1)
                throw new InvalidOperationException("El año del vehículo no es válido.");

            if (vehicle.VehicleTypeId.HasValue)
            {
                var vehicleType = await _vehicleTypeRepository.GetByIdAsync(vehicle.VehicleTypeId.Value);
                if (vehicleType == null)
                    throw new KeyNotFoundException($"Tipo de vehículo con ID {vehicle.VehicleTypeId} no encontrado.");
            }

            if (vehicle.BranchId.HasValue)
            {
                var branch = await _branchRepository.GetByIdAsync(vehicle.BranchId.Value);
                if (branch == null)
                    throw new KeyNotFoundException($"Sucursal con ID {vehicle.BranchId} no encontrada.");
            }
        }
    }
}
