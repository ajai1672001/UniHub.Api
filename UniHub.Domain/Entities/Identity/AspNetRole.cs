using Microsoft.AspNetCore.Identity;
using UniHub.Domain.Interface;

namespace UniHub.Domain.Entities.Identity
{
    public class AspNetRole : IdentityRole<Guid>,IHaveBaseAuditEntityService,IHaveBaseSoftDeleteService, IHaveBaseEntitySerivce
    {
        public string DisplayName { get; set; }

        public Guid? TenantId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime DateCreated {get; set; }

        public DateTime? DateModified { get; set ; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get ; set; }

        public byte[] RowVersion { get ; set; }
    }
}