using UniHub.Domain.Entities;
using UniHub.Domain.Interface;
using UniHub.Dto;
using UniHub.Infrastructure;

namespace UniHub.Service.Services
{
    public class EmailReceiverService : IEmailReceiverService
    {
        private readonly IRepository<EmailReciever> _emailRecieverRepository;

        public EmailReceiverService(IRepository<EmailReciever> emailRecieverRepository)
        {
            _emailRecieverRepository = emailRecieverRepository;
        }

        public async Task SaveEmailReceiversAsync(Guid emailLogId, SendEmailDto dto)
        {
            var sendEmails = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            sendEmails.UnionWith(dto.To ?? Enumerable.Empty<string>());
            sendEmails.UnionWith(dto.Cc ?? Enumerable.Empty<string>());
            sendEmails.UnionWith(dto.Bcc ?? Enumerable.Empty<string>());

            var emailReceivers = sendEmails
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .Select(e => new EmailReciever
                {
                    EmailLogId = emailLogId,
                    Email = e.Trim()
                })
                .ToList();

            if (emailReceivers.Any())
                await _emailRecieverRepository.BulkInsertAsync(emailReceivers);
        }

    }
}