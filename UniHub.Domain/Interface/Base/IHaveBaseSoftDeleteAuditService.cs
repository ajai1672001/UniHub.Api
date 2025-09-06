namespace UniHub.Domain.Interface
{
    public interface IHaveBaseSoftDeleteAuditService
    {
        public bool IsDeleted { get; set; }
    }
}