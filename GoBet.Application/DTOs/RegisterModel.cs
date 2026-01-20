
namespace GoBet.Application.DTOs
{
    public record RegisterModel(
    string Email,
    string Password,
    string FullName,
    string? PhoneNumber
);
}
