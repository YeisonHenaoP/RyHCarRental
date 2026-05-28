using AutoMapper;
using RyHCarRental.API.DTOs;
using RyHCarRental.Domain.Entities;
using RyHCarRental.Domain.Enums;
using RyHCarRental.Domain.Interfaces;

namespace RyHCarRental.API.Services
{
    public class RentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IGenericRepository<Vehicle> _vehicleRepository;
        private readonly IMapper _mapper;

        public RentalService(IRentalRepository rentalRepository,
                             IGenericRepository<Customer> customerRepository,
                             IGenericRepository<Vehicle> vehicleRepository,
                             IMapper mapper)
        {
            _rentalRepository = rentalRepository;
            _customerRepository = customerRepository;
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RentalDto>> GetAllAsync()
        {
            var rentals = await _rentalRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RentalDto>>(rentals);
        }

        public async Task<RentalDto?> GetByIdAsync(int id)
        {
            var rental = await _rentalRepository.GetByIdAsync(id);
            return rental == null ? null : _mapper.Map<RentalDto>(rental);
        }

        public async Task<RentalDto> CreateAsync(RentalCreateDto dto)
        {
            // Validaciones de negocio
            if (dto.StartDate >= dto.EndDate)
                throw new InvalidOperationException("La fecha de inicio debe ser anterior a la fecha de fin.");
            if (dto.StartDate < DateTime.Today)
                throw new InvalidOperationException("No se pueden crear alquileres con fechas pasadas.");
            if (dto.VehicleIds.Count == 0)
                throw new InvalidOperationException("Debe seleccionar al menos un vehículo.");

            // Verificar que el cliente existe
            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
            if (customer == null)
                throw new KeyNotFoundException("Cliente no encontrado.");

            // Verificar disponibilidad de cada vehículo
            foreach (var vehicleId in dto.VehicleIds)
            {
                var available = await _rentalRepository.IsVehicleAvailableAsync(vehicleId, dto.StartDate, dto.EndDate);
                if (!available)
                    throw new InvalidOperationException($"El vehículo con ID {vehicleId} no está disponible en esas fechas.");
            }

            // Calcular costo total: sumar (dailyRate * días) por cada vehículo
            int days = (dto.EndDate - dto.StartDate).Days;
            if (days <= 0) days = 1;

            decimal totalCost = 0;
            var rentalDetails = new List<RentalDetail>();

            foreach (var vehicleId in dto.VehicleIds)
            {
                var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
                if (vehicle == null)
                    throw new KeyNotFoundException($"Vehículo {vehicleId} no existe.");
                if (vehicle.Status != VehicleStatus.Available)
                    throw new InvalidOperationException($"Vehículo {vehicle.Model} no está disponible para alquiler.");

                decimal subTotal = vehicle.DailyRate * days;
                totalCost += subTotal;

                rentalDetails.Add(new RentalDetail
                {
                    VehicleId = vehicleId,
                    SubTotal = subTotal
                });
            }

            var rental = _mapper.Map<Rental>(dto);
            rental.TotalCost = totalCost;
            rental.Status = RentalStatus.Active;
            rental.RentalDetails = rentalDetails;

            await _rentalRepository.AddAsync(rental);
            await _rentalRepository.SaveChangesAsync();

            // Actualizar estado de los vehículos a Rented
            foreach (var vehicleId in dto.VehicleIds)
            {
                var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
                if (vehicle != null)
                {
                    vehicle.Status = VehicleStatus.Rented;
                    _vehicleRepository.Update(vehicle);
                }
            }
            await _vehicleRepository.SaveChangesAsync();

            return _mapper.Map<RentalDto>(rental);
        }

        public async Task CompleteRentalAsync(int rentalId)
        {
            var rental = await _rentalRepository.GetByIdAsync(rentalId);
            if (rental == null) throw new KeyNotFoundException("Alquiler no encontrado.");
            if (rental.Status != RentalStatus.Active)
                throw new InvalidOperationException("Solo se pueden completar alquileres activos.");

            rental.Status = RentalStatus.Completed;
            _rentalRepository.Update(rental);

            // Devolver vehículos a estado Available
            foreach (var detail in rental.RentalDetails)
            {
                var vehicle = await _vehicleRepository.GetByIdAsync(detail.VehicleId);
                if (vehicle != null)
                {
                    vehicle.Status = VehicleStatus.Available;
                    _vehicleRepository.Update(vehicle);
                }
            }
            await _rentalRepository.SaveChangesAsync();
            await _vehicleRepository.SaveChangesAsync();
        }

        public async Task CancelRentalAsync(int rentalId)
        {
            var rental = await _rentalRepository.GetByIdAsync(rentalId);
            if (rental == null) throw new KeyNotFoundException("Alquiler no encontrado.");
            if (rental.Status != RentalStatus.Active)
                throw new InvalidOperationException("Solo se pueden cancelar alquileres activos.");

            rental.Status = RentalStatus.Cancelled;
            _rentalRepository.Update(rental);

            // Liberar vehículos
            foreach (var detail in rental.RentalDetails)
            {
                var vehicle = await _vehicleRepository.GetByIdAsync(detail.VehicleId);
                if (vehicle != null)
                {
                    vehicle.Status = VehicleStatus.Available;
                    _vehicleRepository.Update(vehicle);
                }
            }
            await _rentalRepository.SaveChangesAsync();
            await _vehicleRepository.SaveChangesAsync();
        }
    }
}