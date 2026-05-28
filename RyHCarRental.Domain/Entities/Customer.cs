namespace RyHCarRental.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string DocumentId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        // Relación 1:N con Rental
        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}