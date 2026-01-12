using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniHub.Dto
{
    public class SendEmailTemplateDto
    {
        public List<string> To { get; set; } = new List<string>();
        public List<string> Cc { get; set; } = new List<string>();
        public List<string> BCC { get; set; } = new List<string>();
        public string TemplateName { get; set; }
        public List<PlaceHolderDto> BodyPlaceHolders { get; set; }
        public List<PlaceHolderDto> TextPlaceHolders { get; set; }
        public List<PlaceHolderDto> SubjectPlaceHolders { get; set; }
        public List<string> Attachement { get; set; }

    }

    public  class PlaceHolderDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
