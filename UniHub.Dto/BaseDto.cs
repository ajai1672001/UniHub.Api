namespace UniHub.Dto;

public class BaseTenantSoftDeleteIdAuditDto<T> : BaseSoftDeleteIdDto<T>
{
    public Guid TenantId { get; set; }
}

public class BaseSoftDeleteIdDto<T> : BaseSoftDeleteAuditDto
{
    public T Id { get; set; }
}

public class BaseSoftDeleteAuditDto : BaseAuditDto
{
    public bool IsDeleted { get; set; }
}

public class BaseAuditDto : BaseDto
{
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateModified { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
}

public class BaseDto
{
    public byte[] RowVersion { get; set; }
}