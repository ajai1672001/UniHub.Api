using UniHub.Dto;

namespace UniHub.Domain.Interface
{
    public interface ITenantSettingsService
    {
        Task<BaseResponse<TenantInfoDto>> GetAsync();
        
        Task SaveOrUpdateAsync(TenantInfoDto dto);
    }
}
