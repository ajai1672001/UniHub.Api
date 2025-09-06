using Microsoft.AspNetCore.Identity;
using UniHub.Core.Enum;
using UniHub.Domain.Interface;

namespace UniHub.Domain.Entities.Identity
{
    public class AspNetUser : IdentityUser<Guid>, IHaveBaseAuditEntityService, IHaveBaseSoftDeleteService, IHaveBaseEntitySerivce
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public GenderEnum Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string TimeZone { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        public byte[] RowVersion { get; set; }
    }
}