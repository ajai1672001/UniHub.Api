using Mapster;
using Microsoft.AspNetCore.Identity;
using UniHub.Domain.Entities;
using UniHub.Domain.Entities.Identity;
using UniHub.Domain.Interface;
using UniHub.Dto;
using UniHub.Infrastructure;

namespace UniHub.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly RoleManager<AspNetRole> _roleManager;
        private readonly ITenantUserService _tenantUserService;
        private readonly IHeaderProvider _headerProvider;

        public UserService(
            UserManager<AspNetUser> userManager,
            ITenantUserService tenantUserService,
            RoleManager<AspNetRole> roleManager,
            IHeaderProvider headerProvider = null)
        {
            _userManager = userManager;
            _tenantUserService = tenantUserService;
            _roleManager = roleManager;
            _headerProvider = headerProvider;
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
            else
            {

                user.FirstName = aspNetUser.FirstName;
                user.LastName = aspNetUser.LastName;
                user.Gender = aspNetUser.Gender;
                user.DateOfBirth = aspNetUser.DateOfBirth;
            }
                var userId = result.Id == Guid.Empty ? user.Id : result.Id;
            
            result.TenantUser = await _tenantUserService.SaveTenantUserAsync(aspNetUser.TenantUser, userId);

            return result;
        }

        public async Task CheckEmailIdAlreadyExistAsync(string email, bool checkCurrentTenantOnly = false)
        {
            var user = await GetAspNetUserByEmailAsync(email,false);

            if (user == null)
            {
                return;
            }

            if (await _tenantUserService.GetTenantUserByUserIdAsync(user.Id, checkCurrentTenantOnly))
            {
                throw new ApplicationException($"{email} already in used.");
            }
        }

        public async Task<AspNetUser> GetAspNetUserByEmailAsync(string email, bool exThrew = true)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null && exThrew)
            {
                new ApplicationException("username or password is invalid");
            }

            return user;
        }
    }
}