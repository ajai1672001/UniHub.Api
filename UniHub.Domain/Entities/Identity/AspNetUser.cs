using Microsoft.AspNetCore.Identity;

namespace UniHub.Domain.Entities.Identity
{
    public class AspNetUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public int Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool IsDeleted { get; set; }
    }
}