using Microsoft.AspNetCore.Identity;

namespace UniHub.Entities
{
    public class AspNetRoleClaim : IdentityRoleClaim<Guid>
    {
        public bool IsDeleted { get; set; }
    }
}