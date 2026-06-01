namespace RyHCarRental.API.DTOs.Response
{
    public class RentalResponseDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public List<RentalDetailResponseDto> RentalDetails { get; set; } = new();
    }

    public class RentalDetailResponseDto
    {
        public int VehicleId { get; set; }
        public string VehicleModel { get; set; } = string.Empty;
        public decimal SubTotal { get; set; }
    }
}
