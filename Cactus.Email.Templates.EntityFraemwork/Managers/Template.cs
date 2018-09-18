using System;
using Cactus.Email.Core.Managers;
using Cactus.Email.Core.Senders;

namespace Cactus.Email.Templates.EntityFraemwork.Managers
{
    public class Template : ITemplate<Guid>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string SubjectTemplate { get; set; }

        public string BodyTemplate { get; set; }

        public bool IsBodyHtml { get; set; }

        public string PlainBody { get; set; }

        public string Language { get; set; }

        public EncodingType? HtmlBodyEncoding { get; set; }

        public EncodingType? PlainBodyEncoding { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
