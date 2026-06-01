namespace RyHCarRental.API.DTOs.Response
{
    public class CustomerResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string DocumentId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
