using Microsoft.AspNetCore.Identity;

namespace UniHub.Domain.Entities.Identity
{
    public class AspNetRole : IdentityRole<Guid>
    {
        public string DisplayName { get; set; }

        public Guid? TenantId { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}