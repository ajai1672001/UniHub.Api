using Microsoft.AspNetCore.Identity;

namespace UniHub.Domain.Entities.Identity
{
    public class AspNetRoleClaim : IdentityRoleClaim<Guid>
    {
        public bool IsDeleted { get; set; }
    }
}