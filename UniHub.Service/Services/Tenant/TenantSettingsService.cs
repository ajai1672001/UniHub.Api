using UniHub.Domain.Entities;
using UniHub.Domain.Interface;
using UniHub.Dto;
using UniHub.Infrastructure;

namespace UniHub.Service.Services;

public class TenantSettingsService : ITenantSettingsService
{
    private readonly IRepository<TenantInfo> _tenantInfoRepo;
    private readonly IRepository<SupportInfo> _supportInfoRepo;
    private readonly IRepository<SocialLink> _socialLinkRepo;
    private readonly IUnitOfWork _unitOfWork;

    public TenantSettingsService(
        IRepository<TenantInfo> tenantInfoRepo,
        IRepository<SupportInfo> supportInfoRepo,
        IRepository<SocialLink> socialLinkRepo,
        IUnitOfWork unitOfWork)
    {
        _tenantInfoRepo = tenantInfoRepo;
        _supportInfoRepo = supportInfoRepo;
        _socialLinkRepo = socialLinkRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseResponse<TenantInfoDto>> GetAsync()
    {
        var tenantInfo = (await _tenantInfoRepo
            .GetAsync())
            .FirstOrDefault();
        var supportInfo = (await _supportInfoRepo.GetAsync()).FirstOrDefault();
        var socialLinks = await _socialLinkRepo.GetAsync();

        var dto = new TenantInfoDto();

        if (tenantInfo != null)
        {
            dto.ContactName = tenantInfo.ContactName;
            dto.Email = tenantInfo.Email;
            dto.PhoneNumber = tenantInfo.PhoneNumber;
            dto.AlternatePhoneNumber = tenantInfo.AlternatePhoneNumber;
            dto.AddressLine1 = tenantInfo.AddressLine1;
            dto.AddressLine2 = tenantInfo.AddressLine2;
            dto.City = tenantInfo.City;
            dto.State = tenantInfo.State;
            dto.Country = tenantInfo.Country;
            dto.PostalCode = tenantInfo.PostalCode;
            dto.AboutUs = tenantInfo.AboutUs;
            dto.Vision = tenantInfo.Vision;
            dto.Mission = tenantInfo.Mission;
            dto.Description = tenantInfo.Description;
            dto.LogoUrl = tenantInfo.LogoUrl;
            dto.WebsiteUrl = tenantInfo.WebsiteUrl;
        }

        if (supportInfo != null)
        {
            dto.SupportEmail = supportInfo.SupportEmail;
            dto.SupportPhone = supportInfo.SupportPhone;
            dto.WorkingHours = supportInfo.WorkingHours;
        }

        dto.SocialLinks = socialLinks.Select(x => new SocialLinkDto
        {
            Platform = x.Platform,
            Url = x.Url,
            IsActive = x.IsActive
        }).ToList();

        return BaseResponse<TenantInfoDto>.Success(dto);
    }

    // ---------------- SAVE OR UPDATE ----------------
    public async Task SaveOrUpdateAsync( TenantInfoDto dto)
    {
        // --- TenantInfo ---
        var tenantInfo = (await _tenantInfoRepo.GetAsync()).FirstOrDefault();
        if (tenantInfo == null)
        {
            tenantInfo = new TenantInfo { Id = Guid.NewGuid()};
            await _tenantInfoRepo.InsertAsync(tenantInfo);
        }

        tenantInfo.ContactName = dto.ContactName;
        tenantInfo.Email = dto.Email;
        tenantInfo.PhoneNumber = dto.PhoneNumber;
        tenantInfo.AlternatePhoneNumber = dto.AlternatePhoneNumber;
        tenantInfo.AddressLine1 = dto.AddressLine1;
        tenantInfo.AddressLine2 = dto.AddressLine2;
        tenantInfo.City = dto.City;
        tenantInfo.State = dto.State;
        tenantInfo.Country = dto.Country;
        tenantInfo.PostalCode = dto.PostalCode;
        tenantInfo.AboutUs = dto.AboutUs;
        tenantInfo.Vision = dto.Vision;
        tenantInfo.Mission = dto.Mission;
        tenantInfo.Description = dto.Description;
        tenantInfo.LogoUrl = dto.LogoUrl;
        tenantInfo.WebsiteUrl = dto.WebsiteUrl;

        // --- SupportInfo ---
        var supportInfo = (await _supportInfoRepo.GetAsync()).FirstOrDefault();
        if (supportInfo == null)
        {
            supportInfo = new SupportInfo { Id = Guid.NewGuid() };
            await _supportInfoRepo.InsertAsync(supportInfo);
        }

        supportInfo.SupportEmail = dto.SupportEmail;
        supportInfo.SupportPhone = dto.SupportPhone;
        supportInfo.WorkingHours = dto.WorkingHours;

        // --- SocialLinks ---
        var existingLinks = await _socialLinkRepo.GetAsync();
        _socialLinkRepo.BulkDelete(existingLinks);

        var newLinks = dto.SocialLinks.Select(x => new SocialLink
        {
            Id = Guid.NewGuid(),
            Platform = x.Platform,
            Url = x.Url,
            IsActive = x.IsActive
        });

        await _socialLinkRepo.BulkInsertAsync(newLinks);

        // --- Commit Changes ---
        await _unitOfWork.CommitAsync();
    }
}