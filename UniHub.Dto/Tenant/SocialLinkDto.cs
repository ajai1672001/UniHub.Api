using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniHub.Core.Enum;

namespace UniHub.Dto
{
    public class SocialLinkDto
    {
        public SocialPlatformEnum Platform { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
    }
}
