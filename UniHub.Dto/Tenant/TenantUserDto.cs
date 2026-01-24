using UniHub.Core.Enum;

namespace UniHub.Dto;

public class TenantUserDto:BaseTenantSoftDeleteIdAuditDto<Guid>
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public GenderEnum Gender { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string TimeZone { get; set; } = string.Empty;
    
    public bool IsPrimary { get; set; }

    public Guid AspNetUserId { get; set; }

    public Guid RoleId { get; set; }
}