using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniHub.Domain.Entities;

namespace UniHub.Domain.Interface
{
    public interface IHaveTenantUserIdEntityService
    {
        public Guid TenantUserId { get; set; }
        public TenantUser TenantUser { get; set; }
    }
}
