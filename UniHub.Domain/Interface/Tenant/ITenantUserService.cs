using UniHub.Dto;

namespace UniHub.Domain.Interface;

public interface ITenantUserService
{
    Task<TenantUserDto> SaveTenantUserAsync(TenantUserDto tenantUser, Guid aspNetUserId);

    Task<TenantUserDto> GetTenantUserAsync(Guid id);

    Task<bool> GetTenantUserByUserIdAsync(Guid id, bool currentTenant = false);
}