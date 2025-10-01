using Mapster;
using UniHub.Domain.Entities;
using UniHub.Domain.Interface;
using UniHub.Dto;
using UniHub.Infrastructure;

namespace UniHub.Service.Services
{
    public class TenantUserService : ITenantUserService
    {
        private readonly IRepository<TenantUser> _tenantUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TenantUserService(
            IRepository<TenantUser> tenantUserRepository,
            IUnitOfWork unitOfWork)
        {
            _tenantUserRepository = tenantUserRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TenantUserDto> SaveTenantUserAsync(TenantUserDto dto, Guid aspNetUserId)
        {
            TenantUser tenantUser = (await _tenantUserRepository.GetAsync(e => e.AspNetUserId == aspNetUserId && e.Id == dto.Id))
                .FirstOrDefault();

            if (tenantUser == null)
            {
                dto.Id = Guid.NewGuid();
                
                dto.IsPrimary = await GetTenantUserByUserIdAsync(dto.Id);

                tenantUser = dto.Adapt<TenantUser>();
                tenantUser.AspNetUserId = aspNetUserId;

                await _tenantUserRepository.InsertAsync(tenantUser);
            }
            else
            {
                tenantUser.FirstName = dto.FirstName;
                tenantUser.LastName = dto.LastName;
                tenantUser.Gender = dto.Gender;
                tenantUser.DateOfBirth = dto.DateOfBirth;
                tenantUser.TimeZone = dto.TimeZone;

                _tenantUserRepository.Update(tenantUser);
            }

            await _unitOfWork.CommitAsync();

            return await GetTenantUserAsync(dto.Id);
        }

        public async Task<TenantUserDto> GetTenantUserAsync(Guid id)
        {
            var tenantUser = (await _tenantUserRepository.GetAsync(e => e.Id == id)).FirstOrDefault();

            if (tenantUser == null)
            {
                throw new ApplicationException("Tenant  User not found.");
            }

            return tenantUser.Adapt<TenantUserDto>();
        }

        public async Task<bool> GetTenantUserByUserIdAsync(Guid aspNetUserId, bool currentTenant = false)
        {
            var tenantUser = (await _tenantUserRepository.GetAsync(e => e.AspNetUserId == aspNetUserId, !currentTenant,true)).FirstOrDefault();

            return tenantUser != null;
        }

        public async Task<IEnumerable<TenantUserDto>> GetTenantUsersAsync(Guid UserId)
        {
            var tenantUser = (await _tenantUserRepository
                .GetAsync(e => e.AspNetUserId == UserId)).ToList();

            if (tenantUser == null)
            {
                throw new ApplicationException("Tenant user not found.");
            }

            return tenantUser.Adapt<IEnumerable<TenantUserDto>>();
        }
    }
}