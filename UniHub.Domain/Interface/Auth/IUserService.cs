using UniHub.Dto;

namespace UniHub.Domain.Interface
{
    public interface IUserService
    {
        Task<AspNetUserDto> SaveAspNetUserDto(AspNetUserDto aspNetUser);

        Task CheckEmailIdAlreadyExistAsync(string email, bool checkCurrentTenantOnly = false);
    }
}