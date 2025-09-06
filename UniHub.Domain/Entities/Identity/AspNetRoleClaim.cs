using Microsoft.AspNetCore.Identity;
using UniHub.Domain.Interface;

namespace UniHub.Domain.Entities.Identity
{
    public class AspNetRoleClaim : IdentityRoleClaim<Guid>,IHaveBaseSoftDeleteService, IHaveBaseAuditEntityService, IHaveBaseEntitySerivce
    {
        public bool IsDeleted { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        public byte[] RowVersion { get; set; }
    }
}