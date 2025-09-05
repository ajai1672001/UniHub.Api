using Microsoft.AspNetCore.Identity;

namespace UniHub.Entities
{
    public class AspNetUserClaim : IdentityUserClaim<Guid>
    {
        public bool IsDeleted { get; set; }
    }
}