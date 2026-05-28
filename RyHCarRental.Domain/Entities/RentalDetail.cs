namespace RyHCarRental.Domain.Entities
{
    public class RentalDetail
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public int VehicleId { get; set; }
        public decimal SubTotal { get; set; }

        // Propiedades de navegación
        public Rental Rental { get; set; } = null!;
        public Vehicle Vehicle { get; set; } = null!;
    }
}