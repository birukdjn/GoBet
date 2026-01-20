
<<<<<<< HEAD
namespace GoBet.Application.DTOs
{
    public record DriverRequestDto(string UserId, string LicenseNumber);
=======

namespace GoBet.Application.DTOs
{
    public class DriverRequestDto
    {
        public string UserId { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
    }
>>>>>>> c234042cdbe0294b25c4f396f7dd3818bdcd29e4
}
