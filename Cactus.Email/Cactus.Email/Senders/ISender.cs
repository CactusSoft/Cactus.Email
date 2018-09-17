using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cactus.Email.Core.Senders
{
    public interface ISender
    {
        Task Send(string fromEmail, IEnumerable<string> recipients, IWebPageInfo webPageInfo, string displayName = null);
        Task Send(string fromEmail, string recipient, IWebPageInfo webPageInfo, string displayName = null);
    }
}
