using System.Text;

namespace Cactus.Email.Core.Senders
{
    public interface IEmailContentInfo
    {
        string Subject { get; }

        string HtmlBody { get; }

        string PlainBody { get; }

        string Language { get; }

        Encoding HtmlBodyEncoding { get; }

        Encoding PlainBodyEncoding { get; }
    }
}
