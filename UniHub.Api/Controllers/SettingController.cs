using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniHub.Domain.Interface;
using UniHub.Dto;

namespace UniHub.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly ISettingService _settingService;
        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }
        [HttpPost]
        public async Task<IActionResult> SaveSettingAsync([FromBody] SettingDto dto)
        {
            await _settingService.SaveSettingAsync(dto);
            return Ok();
        }
    }
}
