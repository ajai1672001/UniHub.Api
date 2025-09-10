using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniHub.Dto;

namespace UniHub.Infrastructure
{
    public interface IHeaderProvider
    {
        public Guid TenantId { get; set; }

        public TenantDto CurrentTenant { get; set; }
    }
}
