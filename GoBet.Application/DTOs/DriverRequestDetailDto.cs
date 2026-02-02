
namespace GoBet.Application.DTOs
{
    public class DriverRequestDetailDto
    {
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public DateTime RequestedAt { get; set; }
    }
}
