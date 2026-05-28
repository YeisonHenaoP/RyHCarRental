namespace RyHCarRental.API.DTOs
{
    public class RentalDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public List<RentalDetailDto> RentalDetails { get; set; } = new();
    }

    public class RentalCreateDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CustomerId { get; set; }
        public List<int> VehicleIds { get; set; } = new();
    }

    public class RentalDetailDto
    {
        public int VehicleId { get; set; }
        public string VehicleModel { get; set; } = string.Empty;
        public decimal SubTotal { get; set; }
    }
}