using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniHub.Core.Enum;

namespace UniHub.Domain.Entities
{
    public class SocialLink : BaseTenantSoftDeleteIdAuditEntity<Guid>
    {
        public SocialPlatformEnum Platform { get; set; } // Facebook, Twitter, LinkedIn

        public string Url { get; set; }

        public bool IsActive { get; set; }
    }
}
