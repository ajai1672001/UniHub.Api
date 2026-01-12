namespace UniHub.Domain.Entities;

public class EmailTemplate : BaseSoftDeleteIdEntity<Guid>
{
    public string Name { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }

    public string Text { get; set; }

    public string DefaultEmail { get; set; }

    public bool IsActive { get; set; }

    public Guid? TenantId { get; set; }
}