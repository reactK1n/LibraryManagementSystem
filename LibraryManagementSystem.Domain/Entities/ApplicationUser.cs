using Microsoft.AspNetCore.Identity;

namespace LibraryManagementSystem.Domain.Entities
{
    public class ApplicationUser : IdentityUser<long>
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string? Gender { get; set; }

        public string? ImageUrl { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }
    }
}
