using UniHub.Domain.Entities;

namespace UniHub.Domain.Interface
{
    public interface IHaveBaseTenantSoftDeleteIdAuditEntityService
    {
        public Guid TenantId { get; set; }

        public Tenant Tenant { get; set; }
    }
}