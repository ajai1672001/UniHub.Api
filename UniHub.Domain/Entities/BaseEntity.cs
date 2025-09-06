using UniHub.Domain.Interface;

namespace UniHub.Domain.Entities
{
    public class BaseTenantSoftDeleteIdAuditEntity<T> : BaseSoftDeleteIdEntity<T> , IHaveBaseTenantSoftDeleteIdAuditEntityService where T : struct
    {
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set ; }
    }

    public class BaseSoftDeleteIdEntity<T> : BaseSoftDeleteAuditEntity where T : struct
    {
        public T Id { get; set; }
    }

    public class BaseSoftDeleteAuditEntity : BaseAuditEntity, IHaveBaseSoftDeleteAuditService
    {
        public bool IsDeleted { get; set; }
    }

    public class BaseAuditEntity : BaseEntity, IHaveBaseAuditEntityService
    {
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? DateModified { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }

    public class BaseEntity : IHaveBaseEntitySerivce
    {
        public byte[] RowVersion { get; set; }
    }
}