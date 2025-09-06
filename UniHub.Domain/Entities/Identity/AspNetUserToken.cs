using Microsoft.AspNetCore.Identity;

namespace UniHub.Domain.Entities.Identity
{
    public class AspNetUserToken : IdentityUserToken<Guid>
    {
        public bool IsDeleted { get; set; }
    }
}