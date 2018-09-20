using System.Text;

namespace Cactus.Email.Core.Senders
{
    public class EmailContentInfo : IEmailContentInfo
    {
        public EmailContentInfo(string subject, string htmlBody, string plainBody, string language, Encoding htmlBodyEncoding, Encoding plainBodyEncoding)
        {
            Subject = subject;
            HtmlBody = htmlBody;
            PlainBody = plainBody;
            Language = language;
            HtmlBodyEncoding = htmlBodyEncoding;
            PlainBodyEncoding = plainBodyEncoding;
        }

        public string Subject { get; }
        public string HtmlBody { get; }
        public string PlainBody { get; }
        public string Language { get; }
        public Encoding HtmlBodyEncoding { get; }
        public Encoding PlainBodyEncoding { get; }
    }
}
