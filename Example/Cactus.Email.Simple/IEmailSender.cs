using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cactus.Email.Simple
{
    public interface IEmailSender
    {
        Task Send(Guid templateid, string from, IEnumerable<string> recipients);

        Task Send<TModel>(Guid templateid, string from, IEnumerable<string> recipients, TModel model);
    }
}
