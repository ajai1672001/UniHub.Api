using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniHub.Core.Enum;

namespace UniHub.Domain.Entities
{
    public class EmailLog : BaseTenantSoftDeleteIdAuditEntity<Guid>
    {
        public string Content { get; set; }

        public EmailStatusEnum Status { get; set; }

        public string ErrorMessage { get; set; }

        public ICollection<EmailReciever> EmailRecivers { get; set; }
    }
}