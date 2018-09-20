using System;
using System.Threading.Tasks;
using Cactus.Email.Core.Managers;
using Cactus.Email.Templates.EntityFraemwork.Database;

namespace Cactus.Email.Simple.Services
{
    public interface ITemplatesService
    {
        Task Create(string name, string subjectTemplate, string language, string htmlBodyTemplate, string plainBody, EncodingType? htmlEncoding, EncodingType? plainEncoding);
        Task<ITemplate<Guid>> GetById(Guid templateId);
    }
}
