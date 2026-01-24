using System.Data.Entity;
using UniHub.Core.Enum;
using UniHub.Domain.Entities;
using UniHub.Domain.Interface;
using UniHub.Dto;
using UniHub.Dto.Helper;
using UniHub.Infrastructure;

namespace UniHub.Service.Services
{
    public class EmailLogService : IEmailLogService
    {
        private readonly IRepository<EmailLog> _emailLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailReceiverService _emailReceiverService;
        private readonly ApplicationDbContext _context;

        public EmailLogService(
            IRepository<EmailLog> emailLogRepository,
            IUnitOfWork unitOfWork,
            IEmailReceiverService emailReceiverService,
            ApplicationDbContext context)
        {
            _emailLogRepository = emailLogRepository;
            _unitOfWork = unitOfWork;
            _emailReceiverService = emailReceiverService;
            _context = context;
        }

        public async Task SaveEmailLogAsync(SendEmailDto dto, EmailStatusEnum status, string errorMessage, string payload)
        {
            var id = Guid.NewGuid();

            var emailLog = new EmailLog
            {
                Id = id,
                Status = status,
                ErrorMessage = errorMessage,
                Content = payload
            };

            await _emailLogRepository.InsertAsync(emailLog);
            await _emailReceiverService.SaveEmailReceiversAsync(id, dto);
            await _unitOfWork.CommitAsync();
        }

        public async Task<BaseResponse<PaginationResultDto<EmailLogDto>>> GetEmailLogPagination(PaginationPayloadDto payloadDto)
        {
            IQueryable<EmailReciever> query = _context.EmailRecievers;

            if (!string.IsNullOrWhiteSpace(payloadDto.Search))
            {
                query = query.Where(e => e.Email.Contains(payloadDto.Search));
            }

            var emailLogCount = await query.CountAsync();

            var emailLogs = await query
                .OrderByDescending(e => e.DateCreated)
                .Skip((payloadDto.PageNumber - 1) * payloadDto.PageSize)
                .Take(payloadDto.PageSize)
                .Select(e => new EmailLogDto
                {
                    Status = e.EmailLog.Status,
                    ErrorMessage = e.EmailLog.ErrorMessage,
                    Email = e.Email,
                    SendDate = e.DateCreated
                })
                .ToListAsync();

            return new BaseResponse<PaginationResultDto<EmailLogDto>>
            {
                Code = System.Net.HttpStatusCode.OK,
                IsSuccess = true,
                Message = "Email logs retrieved successfully.",
                Data = new PaginationResultDto<EmailLogDto>
                {
                    TotalCount = emailLogCount,
                    Count = emailLogs.Count,
                    Result = emailLogs
                }
            };
        }
    }
}