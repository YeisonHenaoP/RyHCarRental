namespace RyHCarRental.API.DTOs.Request
{
    public class VehicleRequestDto
    {
        public string Model { get; set; } = string.Empty;
        public string Plate { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal DailyRate { get; set; }
        public int? VehicleTypeId { get; set; }
        public int? BranchId { get; set; }
    }

    public class VehicleUpdateRequestDto : VehicleRequestDto
    {
        public string Status { get; set; } = string.Empty;
    }
}
