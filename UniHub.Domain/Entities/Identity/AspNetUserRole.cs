using Microsoft.AspNetCore.Identity;

namespace UniHub.Domain.Entities.Identity
{
    public class AspNetUserRole : IdentityUserRole<Guid>
    {
        public bool IsDeleted { get; set; }

        public Guid TenantId { get; set; }
    }
}