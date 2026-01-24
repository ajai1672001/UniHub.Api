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
            return Ok(await _settingService.SaveSettingAsync(dto));
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetSettingByNameAsync([FromQuery] string name )
        {
            return Ok(await _settingService.GetSettingByNameAsync(name));
        }
    }
}
