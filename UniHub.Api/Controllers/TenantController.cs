using Microsoft.AspNetCore.Mvc;
using System.Net;
using UniHub.Api.Extension;
using UniHub.Domain.Interface;
using UniHub.Dto;

namespace UniHub.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpPost("signup")]
        [SkipTenantHeader]
        public async Task<IActionResult> SignupTenantAsync([FromBody] TenantSignupDto tenantSignup)
        {
                var result = await _tenantService.SignUpTenantAsync(tenantSignup);

                return Ok(new BaseResponse<TenantSignupDto>(result));
           
        }
    }
}