using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniHub.Domain.Entities.Identity;
using UniHub.Domain.Interface;

namespace UniHub.Domain.Entities
{
    public class TenantUser : BaseTenantSoftDeleteIdAuditEntity<Guid>, IHaveUserIdEntityService
    {
        public Guid AspNetUserId { get; set; }
        public AspNetUser AspNetUser { get; set; }

    }
}
