namespace UniHub.Domain.Entities
{
    public class Setting : BaseTenantSoftDeleteIdAuditEntity<Guid>
    {
        public string Name { get; set; }
        public string Content { get; set; }
    }
}