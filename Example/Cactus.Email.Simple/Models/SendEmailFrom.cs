using System;
using System.Collections.Generic;

namespace Cactus.Email.Simple.Models
{
    public class SendEmailFrom
    {
        public Guid TemplateId { get; set; }

        public string FromAddress { get; set; }

        public List<string> Recipients { get; set; }
    }
}
