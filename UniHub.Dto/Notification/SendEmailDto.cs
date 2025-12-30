using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniHub.Dto;

public class SendEmailDto
{
    public List<string> To { get; set; }
    public List<string> Bcc { get; set; } = null;
    public List<string> Cc { get; set; } = null;
    public string Subject { get; set; }
    public string TextContent { get; set; }
    public string HtmlContent { get; set; }
    public List<string> Attachments { get; set; }
}
