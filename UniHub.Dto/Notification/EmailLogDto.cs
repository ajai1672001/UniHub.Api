using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniHub.Core.Enum;

namespace UniHub.Dto
{
    public class EmailLogDto
    {

        public EmailStatusEnum Status { get; set; }

        public string ErrorMessage { get; set; }

        public string Email { get; set; }

        public DateTime SendDate { get; set; }
    }
}
