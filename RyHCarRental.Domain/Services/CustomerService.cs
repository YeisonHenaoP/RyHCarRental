using RyHCarRental.Domain.Entities;
using RyHCarRental.Domain.Interfaces.Repositories;
using RyHCarRental.Domain.Interfaces.Services;

namespace RyHCarRental.Domain.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IGenericRepository<Customer> _customerRepository;

        public CustomerService(IGenericRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
            => await _customerRepository.GetAllAsync();

        public async Task<Customer?> GetByIdAsync(int id)
            => await _customerRepository.GetByIdAsync(id);

        public async Task<Customer> CreateAsync(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.FullName))
                throw new InvalidOperationException("El nombre completo es obligatorio.");
            if (string.IsNullOrWhiteSpace(customer.DocumentId))
                throw new InvalidOperationException("El documento es obligatorio.");
            if (string.IsNullOrWhiteSpace(customer.Email))
                throw new InvalidOperationException("El email es obligatorio.");

            await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveChangesAsync();
            return customer;
        }
    }
}
