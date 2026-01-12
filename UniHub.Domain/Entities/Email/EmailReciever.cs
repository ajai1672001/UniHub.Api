using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniHub.Domain.Entities
{
    public class EmailReciever: BaseTenantSoftDeleteIdAuditEntity<Guid>
    {
        public Guid EmailLogId { get; set; }
        public string Email { get; set; }
        public EmailLog EmailLog { get; set; }
    }
}
