using UniHub.Dto;

namespace UniHub.Domain.Interface
{
    public interface ITenantService
    {
        Task<TenantSignupDto> SignUpTenantAsync(TenantSignupDto signupDto);
    }
}