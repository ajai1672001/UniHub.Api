using UniHub.Dto;

namespace UniHub.Domain.Interface
{
    public interface IEmailTemplateService
    {
        Task<SendEmailDto> GetSendEmailDtoAsync(SendEmailTemplateDto sendEmail);
    }
}