namespace UniHub.Domain.Interface
{
    public interface IHaveBaseAuditEntityService
    {
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}