using RyHCarRental.Domain.Entities;
using RyHCarRental.Domain.Enums;
using RyHCarRental.Domain.Interfaces.Repositories;
using RyHCarRental.Domain.Interfaces.Services;

namespace RyHCarRental.Domain.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IGenericRepository<Vehicle> _vehicleRepository;

        public RentalService(
            IRentalRepository rentalRepository,
            IGenericRepository<Customer> customerRepository,
            IGenericRepository<Vehicle> vehicleRepository)
        {
            _rentalRepository = rentalRepository;
            _customerRepository = customerRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<IEnumerable<Rental>> GetAllAsync()
            => await _rentalRepository.GetAllAsync();

        public async Task<Rental?> GetByIdAsync(int id)
            => await _rentalRepository.GetByIdAsync(id);

        public async Task<Rental> CreateAsync(Rental rental, IEnumerable<int> vehicleIds)
        {
            var vehicleIdList = vehicleIds.ToList();

            if (rental.StartDate >= rental.EndDate)
                throw new InvalidOperationException("La fecha de inicio debe ser anterior a la fecha de fin.");
            if (rental.StartDate < DateTime.Today)
                throw new InvalidOperationException("No se pueden crear alquileres con fechas pasadas.");
            if (vehicleIdList.Count == 0)
                throw new InvalidOperationException("Debe seleccionar al menos un vehículo.");

            var customer = await _customerRepository.GetByIdAsync(rental.CustomerId);
            if (customer == null)
                throw new KeyNotFoundException("Cliente no encontrado.");

            foreach (var vehicleId in vehicleIdList)
            {
                var available = await _rentalRepository.IsVehicleAvailableAsync(
                    vehicleId, rental.StartDate, rental.EndDate);
                if (!available)
                    throw new InvalidOperationException(
                        $"El vehículo con ID {vehicleId} no está disponible en esas fechas.");
            }

            int days = (rental.EndDate - rental.StartDate).Days;
            if (days <= 0) days = 1;

            decimal totalCost = 0;
            var rentalDetails = new List<RentalDetail>();

            foreach (var vehicleId in vehicleIdList)
            {
                var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
                if (vehicle == null)
                    throw new KeyNotFoundException($"Vehículo {vehicleId} no existe.");
                if (vehicle.Status != VehicleStatus.Available)
                    throw new InvalidOperationException(
                        $"Vehículo {vehicle.Model} no está disponible para alquiler.");

                decimal subTotal = vehicle.DailyRate * days;
                totalCost += subTotal;

                rentalDetails.Add(new RentalDetail
                {
                    VehicleId = vehicleId,
                    SubTotal = subTotal
                });
            }

            rental.TotalCost = totalCost;
            rental.Status = RentalStatus.Active;
            rental.RentalDetails = rentalDetails;

            await _rentalRepository.AddAsync(rental);
            await _rentalRepository.SaveChangesAsync();

            foreach (var vehicleId in vehicleIdList)
            {
                var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
                if (vehicle != null)
                {
                    vehicle.Status = VehicleStatus.Rented;
                    _vehicleRepository.Update(vehicle);
                }
            }
            await _vehicleRepository.SaveChangesAsync();

            return rental;
        }

        public async Task CompleteRentalAsync(int rentalId)
        {
            var rental = await _rentalRepository.GetByIdAsync(rentalId);
            if (rental == null)
                throw new KeyNotFoundException("Alquiler no encontrado.");
            if (rental.Status != RentalStatus.Active)
                throw new InvalidOperationException("Solo se pueden completar alquileres activos.");

            rental.Status = RentalStatus.Completed;
            _rentalRepository.Update(rental);

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
            if (rental == null)
                throw new KeyNotFoundException("Alquiler no encontrado.");
            if (rental.Status != RentalStatus.Active)
                throw new InvalidOperationException("Solo se pueden cancelar alquileres activos.");

            rental.Status = RentalStatus.Cancelled;
            _rentalRepository.Update(rental);

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
