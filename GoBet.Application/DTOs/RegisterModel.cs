
namespace GoBet.Application.DTOs
{
<<<<<<< HEAD
    public record RegisterModel(
    string Email,
    string Password,
    string FullName,
    string? PhoneNumber
);
=======
    public class RegisterModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }
>>>>>>> c234042cdbe0294b25c4f396f7dd3818bdcd29e4
}
