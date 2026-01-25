using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UniHub.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using UniHub.Domain.Interface;
using UniHub.Dto;

[ApiController]
[Route("api/v1/tenant-info")]
public class TenantInfoController : ControllerBase
{
    private readonly ITenantSettingsService _settingsService;

    public TenantInfoController(ITenantSettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    /// <summary>
    /// Get Tenant Info + Support Info + Social Links
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _settingsService.GetAsync();
        return Ok(result);
    }

    /// <summary>
    /// Save or Update Tenant Info + Support Info + Social Links
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> SaveOrUpdate([FromBody] TenantInfoDto dto)
    {
        if (dto == null)
            return BadRequest("Settings data is required.");

        await _settingsService.SaveOrUpdateAsync( dto);

        return Ok(BaseResponse<object>.Success( new { message = "Settings saved successfully." }));
    }

}
