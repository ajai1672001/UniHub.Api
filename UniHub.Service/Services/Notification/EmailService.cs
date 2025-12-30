using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using UniHub.Domain.Interface;
using UniHub.Dto;

namespace UniHub.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly BrevoSettingsDto _settings;

        public EmailService(IConfiguration configuration)
        {
            //_settings = configuration.GetSection("Brevo").GetType<BrevoSettingsDto>();
        }

        public async Task SendEmailAsync(SendEmailDto sendEmail)
        {
            var url = _settings.EmailUrl;

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("api-key", _settings.ApiKey);

            var payload = new Dictionary<string, object>
            {
                ["sender"] = new { name = _settings.FromName, email = _settings.FromEmail },
                ["to"] = sendEmail.To?.Select(email => new { email, name = email }).ToList(),
                ["subject"] = sendEmail.Subject,
                ["htmlContent"] = sendEmail.HtmlContent,
                ["textContent"] = sendEmail.TextContent
            };

            // Only add CC if provided
            if (sendEmail.Cc != null && sendEmail.Cc.Any())
                payload["cc"] = sendEmail.Cc.Select(email => new { email, name = email }).ToList();

            // Only add BCC if provided
            if (sendEmail.Bcc != null && sendEmail.Bcc.Any())
                payload["bcc"] = sendEmail.Bcc.Select(email => new { email, name = email }).ToList();

            if (sendEmail.Attachments != null && sendEmail.Attachments.Any())
            {
                payload["attachment"] = sendEmail.Attachments.Select(url => new {
                    url,
                    name = $"{DateTime.UtcNow:yyyyMMdd_HHmmss}{Path.GetExtension(url)}"
                }).ToList();
            }
            var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);

            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Brevo API error: {response.StatusCode} - {result}");
            }
        }
    }
}
