namespace UniHub.Entities
{
    public class BaseTenantSoftDeleteIdAuditEntity<T> : BaseSoftDeleteIdEntity<T>
    {
        public Guid TenantId { get; set; }
    }

    public class BaseSoftDeleteIdEntity<T> : BaseSoftDeleteAuditEntity
    {
        public T Id { get; set; }
    }

    public class BaseSoftDeleteAuditEntity : BaseAuditEntity
    {
        public bool IsDeleted { get; set; }
    }

    public class BaseAuditEntity : BaseEntity
    {
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? DateModified { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }

    public abstract class BaseEntity
    {
    }
}