using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniHub.Domain.Interface;
using UniHub.Dto.Helper;

namespace UniHub.Api.Controllers.Notification
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmailLogController : ControllerBase
    {
        private readonly IEmailLogService _emailLogService;
        public EmailLogController(IEmailLogService emailLogService)
        {
            _emailLogService = emailLogService;
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetEmailLogs([FromQuery] PaginationPayloadDto payloadDto)
        {
                return Ok(await _emailLogService.GetEmailLogPagination(payloadDto));
        }
    }
}
