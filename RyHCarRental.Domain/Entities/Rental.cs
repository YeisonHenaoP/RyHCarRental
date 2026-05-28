using RyHCarRental.Domain.Enums;

namespace RyHCarRental.Domain.Entities
{
    public class Rental
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalCost { get; set; }
        public RentalStatus Status { get; set; } = RentalStatus.Active;

        // Llave foránea para Customer (relación 1:N)
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        // Relación N:M con Vehicle a través de RentalDetail
        public ICollection<RentalDetail> RentalDetails { get; set; } = new List<RentalDetail>();
    }
}