using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniHub.Domain.Entities
{
    public class TenantInfo : BaseTenantSoftDeleteIdAuditEntity<Guid>
    {
        public string ContactName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string AlternatePhoneNumber { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string PostalCode { get; set; }

        public string AboutUs { get; set; }

        public string Vision { get; set; }

        public string Mission { get; set; }

        public string Description { get; set; }

        public string LogoUrl { get; set; }

        public string WebsiteUrl { get; set; }
    }
}
