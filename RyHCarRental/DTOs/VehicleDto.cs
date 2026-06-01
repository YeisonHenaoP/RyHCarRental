namespace RyHCarRental.API.DTOs
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public string Plate { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal DailyRate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int? VehicleTypeId { get; set; }
        public string? VehicleTypeName { get; set; }
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
    }

    public class VehicleCreateDto
    {
        public string Model { get; set; } = string.Empty;
        public string Plate { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal DailyRate { get; set; }
        public int? VehicleTypeId { get; set; }
        public int? BranchId { get; set; }
    }

    public class VehicleUpdateDto : VehicleCreateDto
    {
        public string Status { get; set; } = string.Empty;
    }
}
