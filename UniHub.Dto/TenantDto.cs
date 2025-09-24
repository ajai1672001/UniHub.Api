namespace UniHub.Dto;

public class TenantDto : BaseSoftDeleteIdDto<Guid>
{
    public string Name { get; set; }
    public string TimeZone { get; set; }
}