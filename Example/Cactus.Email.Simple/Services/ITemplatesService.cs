using System;
using System.Threading.Tasks;
using Cactus.Email.Core.Senders;
using Cactus.Email.Templates.EntityFraemwork.Managers;

namespace Cactus.Email.Simple.Services
{
    public interface ITemplatesService
    {
        Task Create(string name, string subjectTemplate, string language, string htmlBodyTemplate, string plainBody, EncodingType? htmlEncoding, EncodingType? plainEncoding);
        Task<Template> GetById(Guid templateId);
    }
}
