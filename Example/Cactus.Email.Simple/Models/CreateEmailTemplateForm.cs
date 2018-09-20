using Cactus.Email.Templates.EntityFraemwork.Database;

namespace Cactus.Email.Simple.Models
{
    public class CreateEmailTemplateForm
    {
        public string Name { get; set; }

        public string Subject { get; set; }

        public string Language { get; set; }

        public string HtmlBody { get; set; }

        public string PlainBody { get; set; }

        public EncodingType? HtmlBodyEncoding { get; set; }

        public EncodingType? PlainBodyEncoding { get; set; }
    }
}
