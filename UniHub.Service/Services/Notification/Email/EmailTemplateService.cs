using UniHub.Domain.Entities;
using UniHub.Domain.Interface;
using UniHub.Dto;
using UniHub.Infrastructure;

namespace UniHub.Service.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IRepository<EmailTemplate> _templateRepository;

        public EmailTemplateService(IRepository<EmailTemplate> templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public async Task<SendEmailDto> GetSendEmailDtoAsync(SendEmailTemplateDto sendEmail)
        {
            var template = (await _templateRepository
                .GetAsync(et => et.Name == sendEmail.TemplateName && et.IsActive))
                .FirstOrDefault();

            if (template == null)
            {
                throw new Exception($"Email template '{sendEmail.TemplateName}' not found or inactive.");
            }

            if (string.IsNullOrEmpty(template.DefaultEmail) == false)
            {
                sendEmail.BCC.Add(template.DefaultEmail);
            }

            var sendEmailDto = new SendEmailDto
            {
                To = sendEmail.To,
                Cc = sendEmail.Cc,
                Bcc = sendEmail.BCC,
                Subject = ReplaceText(template.Subject, sendEmail.SubjectPlaceHolders),
                HtmlContent = ReplaceText(template.Body, sendEmail.BodyPlaceHolders),
                TextContent = ReplaceText(template.Text, sendEmail.TextPlaceHolders),
                Attachments = sendEmail.Attachement
            };

            return sendEmailDto;
        }

        private string ReplaceText(string text, List<PlaceHolderDto> placeHolders)
        {
            if (string.IsNullOrEmpty(text) || placeHolders == null || placeHolders.Count == 0)
            {
                return text;
            }

            foreach (var placeholder in placeHolders)
            {
                if (!string.IsNullOrEmpty(placeholder.Key))
                {
                    text = text.Replace(placeholder.Key, placeholder.Value ?? string.Empty);
                }
            }

            return text;
        }
    }
}