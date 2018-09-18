using System;
using System.Threading.Tasks;
using Cactus.Email.Core.Repositories;
using Cactus.Email.Core.Senders;

namespace Cactus.Email.Simple.Services
{
    public interface ITemplatesService
    {
        Task Create(string name, string subjectTemplate, string language, string htmlBodyTemplate, string plainBody, EncodingType? htmlEncoding, EncodingType? plainEncoding);
        Task<ITemplate> GetById(Guid templateId);
    }
}
