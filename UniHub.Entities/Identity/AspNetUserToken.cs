using Microsoft.AspNetCore.Identity;

namespace UniHub.Entities
{
    public class AspNetUserToken : IdentityUserToken<Guid>
    {
        public bool IsDeleted { get; set; }
    }
}