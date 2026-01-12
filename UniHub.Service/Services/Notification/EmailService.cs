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
    private readonly IRepository<EmailTemplate> _emailTemplatRepository;
    private readonly IRepository<EmailLog> _emailLogRepository;
    private readonly IRepository<EmailReciever> _emailReciverRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EmailService(
        IConfiguration configuration,
        IRepository<EmailTemplate> emailTemplatRepository,
        IRepository<EmailLog> emailLogRepository,
        IRepository<EmailReciever> emailReciverRepository,
        IUnitOfWork unitOfWork)
    {
        _settings = configuration.GetSection("Brevo").Get<BrevoSettingsDto>();
        _emailTemplatRepository = emailTemplatRepository;
        _emailLogRepository = emailLogRepository;
        _emailReciverRepository = emailReciverRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task SendEmailFromTemplateAsync(SendEmailTemplateDto sendEmail)
    {
        var template = (await _emailTemplatRepository
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

        await SendEmailAsync(sendEmailDto);
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

    public async Task SendEmailAsync(SendEmailDto sendEmail)
    {
        var url = _settings.EmailUrl;

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
                name = Path.GetFileName(fileUrl) // Use actual filename from URL if possible
            }).ToList();
        }

        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        });

        try
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            // Set a timeout for safety
            httpClient.Timeout = TimeSpan.FromSeconds(30);

            var response = await httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetail = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Brevo API Error ({response.StatusCode}): {errorDetail}");
            }

            await SaveEmailLogAsync(sendEmail, EmailStatusEnum.Success, string.Empty, json);
        }
        catch (Exception ex)
        {
            await SaveEmailLogAsync(sendEmail, EmailStatusEnum.Failed, string.Empty, json);
            throw ex;
        }
    }

    private async Task SaveEmailLogAsync(
     SendEmailDto dto,
     EmailStatusEnum status,
     string errorMessage,
     string payload)
    {
        var sendEmails = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        sendEmails.UnionWith(dto.To ?? Enumerable.Empty<string>());
        sendEmails.UnionWith(dto.Cc ?? Enumerable.Empty<string>());
        sendEmails.UnionWith(dto.Bcc ?? Enumerable.Empty<string>());

        var id = Guid.NewGuid();

        var emailLog = new EmailLog
        {
            Id = id,
            Status = status,
            ErrorMessage = errorMessage,
            Content = payload
        };

        await _emailLogRepository.InsertAsync(emailLog);

        var emailReceivers = sendEmails
            .Where(e => !string.IsNullOrWhiteSpace(e))
            .Select(e => new EmailReciever
            {
                EmailLogId = id,
                Email = e.Trim()
            })
            .ToList();

        await _emailReciverRepository.BulkInsertAsync(emailReceivers);

        await _unitOfWork.CommitAsync();
    }
}