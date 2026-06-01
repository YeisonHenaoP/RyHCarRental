namespace RyHCarRental.API.DTOs.Request
{
    public class RentalRequestDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CustomerId { get; set; }
        public List<int> VehicleIds { get; set; } = new();
    }
}
