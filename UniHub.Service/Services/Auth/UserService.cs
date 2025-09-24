using Mapster;
using Microsoft.AspNetCore.Identity;
using UniHub.Domain.Entities.Identity;
using UniHub.Domain.Interface;
using UniHub.Dto;

namespace UniHub.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly ITenantUserService _tenantUserService;

        public UserService(
            UserManager<AspNetUser> userManager,
            ITenantUserService tenantUserService)
        {
            _userManager = userManager;
            _tenantUserService = tenantUserService;
        }

        public async Task<AspNetUserDto> SaveAspNetUserDto(AspNetUserDto aspNetUser)
        {
            var user = await _userManager.FindByEmailAsync(aspNetUser.Email);

            AspNetUserDto result = new AspNetUserDto();

            if (user == null)
            {
                aspNetUser.Password = string.IsNullOrEmpty(aspNetUser.Password) ? "@j1thP@$$w0rd" : aspNetUser.Password;

                user = aspNetUser.Adapt<AspNetUser>();

                user.UserName = user.Email;

                result = (await _userManager.CreateAsync(user, aspNetUser.Password))
                    .Adapt<AspNetUserDto>();
            }
            

            result.TenantUser = await _tenantUserService.SaveTenantUserAsync(aspNetUser.TenantUser, result.Id == Guid.Empty ? user.Id :result.Id);

            return result;
        }

        public async Task CheckEmailIdAlreadyExistAsync(string email, bool checkCurrentTenantOnly = false)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return;
            }

            if (await _tenantUserService.GetTenantUserByUserIdAsync(user.Id, checkCurrentTenantOnly))
            {
                throw new ApplicationException($"{email} already in used.");
            }
        }
    }
}