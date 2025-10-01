using UniHub.Domain.Entities.Identity;
using UniHub.Dto;

namespace UniHub.Domain.Interface
{
    public interface IUserService
    {
        Task<AspNetUserDto> SaveAspNetUserDto(AspNetUserDto aspNetUser);

        Task CheckEmailIdAlreadyExistAsync(string email, bool checkCurrentTenantOnly = false);

        Task<AspNetUser> GetAspNetUserByEmailAsync(string email, bool exThrew = true);
    }
}