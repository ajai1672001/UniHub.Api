using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniHub.Domain.Entities
{
    public class SupportInfo : BaseTenantSoftDeleteIdAuditEntity<Guid>
    {
        public string SupportEmail { get; set; }

        public string SupportPhone { get; set; }

        public string WorkingHours { get; set; }
    }
}
