using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniHub.Domain.Entities
{
    public class EmailReciever: BaseSoftDeleteIdEntity<Guid>
    {
        public Guid EmailLogId { get; set; }
        public string Email { get; set; }
        public virtual EmailLog? EmailLog { get; set; }
    }
}
