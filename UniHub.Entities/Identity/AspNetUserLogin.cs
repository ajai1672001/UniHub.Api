using Microsoft.AspNetCore.Identity;

namespace UniHub.Entities
{
    public class AspNetUserLogin : IdentityUserLogin<Guid>
    {
        public bool IsDeleted { get; set; }
    }
}