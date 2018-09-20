using System;
using Cactus.Email.Templates.EntityFraemwork.Database;

namespace Cactus.Email.Templates.EntityFraemwork.Managers
{
    public class Template
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string SubjectTemplate { get; set; }

        public string HtmlBodyTemplate { get; set; }

        public string PlainBody { get; set; }

        public string Language { get; set; }

        public EncodingType? HtmlBodyEncoding { get; set; }

        public EncodingType? PlainBodyEncoding { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
