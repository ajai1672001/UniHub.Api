using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniHub.Api.Extension;
using UniHub.Domain.Interface;
using UniHub.Dto;

namespace UniHub.Api.Controllers.Notification
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmailAsync([FromBody] SendEmailDto sendEmail)
        {
            await _emailService.SendEmailAsync(sendEmail);
            return Ok();
        }

        [HttpPost("template-send")]
        [SkipTenantHeader]
        public async Task<IActionResult> SendEmailTemplateAsync([FromBody] SendEmailTemplateDto sendEmail)
        {
            await _emailService.SendEmailFromTemplateAsync(sendEmail);
            return Ok();
        }
    }
}
