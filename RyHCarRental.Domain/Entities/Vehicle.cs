using RyHCarRental.Domain.Enums;

namespace RyHCarRental.Domain.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public string Plate { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal DailyRate { get; set; }
        public VehicleStatus Status { get; set; } = VehicleStatus.Available;

        // Llaves foráneas (luego tu compañero agregará VehicleTypeId y BranchId)
        public int? VehicleTypeId { get; set; }
        public int? BranchId { get; set; }
    }
}