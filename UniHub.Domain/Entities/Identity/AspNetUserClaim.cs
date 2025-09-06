using Microsoft.AspNetCore.Identity;

namespace UniHub.Domain.Entities.Identity
{
    public class AspNetUserClaim : IdentityUserClaim<Guid>
    {
        public bool IsDeleted { get; set; }
    }
}