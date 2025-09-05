using Microsoft.AspNetCore.Identity;

namespace UniHub.Entities
{
    public class AspNetUserRole : IdentityUserRole<Guid>
    {
        public bool IsDeleted { get; set; }

        public Guid TenantId { get; set; }
    }
}