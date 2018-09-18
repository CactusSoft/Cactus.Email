namespace Cactus.Email.Core.Senders
{
    public class WebPageInfo : IWebPageInfo
    {
        public WebPageInfo(string subject, string body, string plainBody, bool isBodyHtml, string language, EncodingType? htmlBodyEncoding, EncodingType? plainBodyEncoding)
        {
            Subject = subject;
            Body = body;
            IsBodyHtml = isBodyHtml;
            PlainBody = plainBody;
            Language = language;
            HtmlBodyEncoding = htmlBodyEncoding;
            PlainBodyEncoding = plainBodyEncoding;
        }

        public string Subject { get; }
        public string Body { get; }
        public bool IsBodyHtml { get; set; }
        public string PlainBody { get; }
        public string Language { get; }
        public EncodingType? HtmlBodyEncoding { get; }
        public EncodingType? PlainBodyEncoding { get; }
    }
}
