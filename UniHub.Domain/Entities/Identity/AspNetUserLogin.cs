using Microsoft.AspNetCore.Identity;
using UniHub.Domain.Interface;

namespace UniHub.Domain.Entities.Identity
{
    public class AspNetUserLogin : IdentityUserLogin<Guid>, IHaveBaseAuditEntityService, IHaveBaseEntitySerivce,IHaveBaseSoftDeleteService
    {
        public DateTime DateCreated { get ; set ; }
        public DateTime? DateModified { get ; set ; }
        public Guid? CreatedBy { get ; set ; }
        public Guid? UpdatedBy { get ; set ; }
        public byte[] RowVersion { get ; set ; }
        public bool IsDeleted { get ; set ; }
    }
}