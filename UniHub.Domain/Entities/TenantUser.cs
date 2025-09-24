using UniHub.Core.Enum;
using UniHub.Domain.Entities.Identity;
using UniHub.Domain.Interface;

namespace UniHub.Domain.Entities
{
    public class TenantUser : BaseTenantSoftDeleteIdAuditEntity<Guid>, IHaveUserIdEntityService
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public GenderEnum Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string TimeZone { get; set; }

        public Guid AspNetUserId { get; set; }

        public Guid RoleId { get; set; }

        public AspNetUser AspNetUser { get; set; }

        public AspNetRole AspNetRole { get; set; }
    }
}