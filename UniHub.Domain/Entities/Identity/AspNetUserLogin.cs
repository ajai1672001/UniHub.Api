using Microsoft.AspNetCore.Identity;

namespace UniHub.Domain.Entities.Identity
{
    public class AspNetUserLogin : IdentityUserLogin<Guid>
    {
        public bool IsDeleted { get; set; }
    }
}