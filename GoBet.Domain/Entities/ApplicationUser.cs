using Microsoft.AspNetCore.Identity;

namespace GoBet.Domain.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDriverApproved { get; set; } = false;
        public string? LicenseNumber { get; set; }
        public DateTime? LastLoginDate { get; set; }

    }
}
