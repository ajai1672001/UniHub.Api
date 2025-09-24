using Mapster;
using UniHub.Domain.Entities;
using UniHub.Domain.Interface;
using UniHub.Dto;
using UniHub.Infrastructure;

namespace UniHub.Service.Services
{
    public class TenantService : ITenantService
    {
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeaderProvider _headerProvider;

        public TenantService(IRepository<Tenant> tenantRepository,
            IUnitOfWork unitOfWork,
            IUserService userService,
            IHeaderProvider headerProvider)
        {
            _tenantRepository = tenantRepository;
            _unitOfWork = unitOfWork;
            _userService = userService;
            _headerProvider = headerProvider;
        }

        public async Task<TenantSignupDto> SignUpTenantAsync(TenantSignupDto signupDto)
        {
            await CheckDuplicateNameAsync(signupDto.Name);

            await _userService.CheckEmailIdAlreadyExistAsync(signupDto.AspNetUser.Email);

            var tenant = signupDto.Adapt<Tenant>();
            tenant.Id = Guid.NewGuid();

            await _tenantRepository.InsertAsync(tenant);
            
            await _unitOfWork.CommitAsync();

            var result = (await GetAsync(tenant.Id)).Adapt<TenantSignupDto>();

            var newTeant = new TenantDto
            {
                Name = result.Name,
                Id = tenant.Id,
                TimeZone = result.TimeZone,
            };

            _headerProvider.ResetTenant(tenant.Id, newTeant);

            result.AspNetUser = await _userService.SaveAspNetUserDto(signupDto.AspNetUser);

            return result;
        }

        private async Task CheckDuplicateNameAsync(string name)
        {
            var tenant = (await _tenantRepository.GetAsync(e => e.Name == name)).FirstOrDefault();

            if (tenant != null)
            {
                throw new ApplicationException($"{name} name is already used.");
            }
        }

        private async Task<Tenant> GetAsync(Guid id, bool throwEx = false)
        {
            Tenant tenant = (await _tenantRepository.GetAsync(e => e.Id == id)).FirstOrDefault();

            if (throwEx)
            {
                throw new ApplicationException("Tenant not found.");
            }

            return tenant;
        }
    }
}