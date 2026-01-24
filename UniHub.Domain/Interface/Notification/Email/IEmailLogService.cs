using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniHub.Core.Enum;
using UniHub.Dto;
using UniHub.Dto.Helper;

namespace UniHub.Domain.Interface
{
    public interface IEmailLogService
    {
        Task SaveEmailLogAsync(SendEmailDto dto, EmailStatusEnum status, string errorMessage, string payload);
        Task<BaseResponse<PaginationResultDto<EmailLogDto>>> GetEmailLogPagination(PaginationPayloadDto payloadDto);
    }
}
