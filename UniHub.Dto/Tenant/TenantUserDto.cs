using UniHub.Core.Enum;

namespace UniHub.Dto;

public class TenantUserDto:BaseTenantSoftDeleteIdAuditDto<Guid>
{
    public string TimeZone { get; set; } = string.Empty;
    
    public bool IsPrimary { get; set; }

    public Guid AspNetUserId { get; set; }

    public Guid RoleId { get; set; }
}