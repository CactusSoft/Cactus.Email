namespace Cactus.Email.Core.Senders
{
    public enum EncodingType
    {
        Default,
        Unicode,
        UTF8,
        ASCII,
        UTF7,
        UTF32,
        BigEndianUnicode
    }

    public interface IWebPageInfo
    {
        string Subject { get; }

        string Body { get; }

        bool IsBodyHtml { get; set; }

        string PlainBody { get; }

        string Language { get; }

        EncodingType? HtmlBodyEncoding { get; }

        EncodingType? PlainBodyEncoding { get; }
    }
}
