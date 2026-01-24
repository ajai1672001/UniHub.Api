using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using UniHub.Core.Enum;
using UniHub.Domain.Entities;
using UniHub.Domain.Interface;
using UniHub.Dto;
using UniHub.Infrastructure;

namespace UniHub.Service.Services;

public class EmailService : IEmailService
{
    private readonly BrevoSettingsDto _settings;
    private readonly IEmailLogService _emailLogService;
    private IEmailTemplateService _emailTemplateService;

    public EmailService(
        IConfiguration configuration,
        IEmailTemplateService emailTemplateService,
        IEmailLogService emailLogService)
    {
        _settings = configuration.GetSection("Brevo").Get<BrevoSettingsDto>();
        _emailTemplateService = emailTemplateService;
        _emailLogService = emailLogService;
    }

    public async Task SendEmailFromTemplateAsync(SendEmailTemplateDto sendEmail)
    {
        var template = await _emailTemplateService.GetSendEmailDtoAsync(sendEmail);

        await SendEmailAsync(template);
    }

    public async Task SendEmailAsync(SendEmailDto sendEmail)
    {
        var url = _settings.EmailUrl;
        string json = null;

        try
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("api-key", _settings.ApiKey);

            var payload = new Dictionary<string, object>
            {
                ["sender"] = new { name = _settings.FromName, email = _settings.FromEmail },
                ["to"] = sendEmail.To?.Select(e => new { email = e }).ToList(),
                ["subject"] = sendEmail.Subject,
                ["htmlContent"] = sendEmail.HtmlContent,
                ["textContent"] = sendEmail.TextContent
            };

            if (sendEmail.Cc?.Any() == true)
                payload["cc"] = sendEmail.Cc.Select(e => new { email = e }).ToList();

            if (sendEmail.Bcc?.Any() == true)
                payload["bcc"] = sendEmail.Bcc.Select(e => new { email = e }).ToList();

            if (sendEmail.Attachments?.Any() == true)
            {
                payload["attachment"] = sendEmail.Attachments.Select(fileUrl => new
                {
                    url = fileUrl,
                    name = Path.GetFileName(fileUrl)
                }).ToList();
            }

            json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.Timeout = TimeSpan.FromSeconds(30);

            var response = await httpClient.PostAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Brevo API Error ({response.StatusCode}): {responseContent}");
            }

            // ✅ Log success
            await SafeLogAsync(sendEmail, EmailStatusEnum.Success, string.Empty, json);
        }
        catch (Exception ex)
        {
            // ✅ Always attempt logging failure
            await SafeLogAsync(sendEmail, EmailStatusEnum.Failed, ex.Message, json);

            throw; // ✅ preserves stack trace
        }
    }


    private async Task SafeLogAsync(SendEmailDto dto, EmailStatusEnum status, string error, string payload)
    {
        try
        {
            await _emailLogService.SaveEmailLogAsync(dto, status, error, payload);
        }
        catch (Exception logEx)
        {
            // Last fallback: log to file / console / monitoring
            Console.WriteLine("Email log failed: " + logEx.Message);
        }
    }

}