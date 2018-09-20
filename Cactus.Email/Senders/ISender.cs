using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cactus.Email.Core.Senders
{
    public interface ISender
    {
        Task Send(string fromEmail, IEnumerable<string> recipients, IEmailContentInfo emailContentInfo, string displayName = null);
        Task Send(string fromEmail, string recipient, IEmailContentInfo emailContentInfo, string displayName = null);
    }
}
