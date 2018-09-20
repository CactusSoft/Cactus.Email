using System;
using System.Threading.Tasks;
using Cactus.Email.Core.Managers;
using Cactus.Email.Templates.EntityFraemwork;
using Cactus.Email.Templates.EntityFraemwork.Database;
using Cactus.Email.Templates.EntityFraemwork.Managers;

namespace Cactus.Email.Simple.Services
{
    public class TemplatesService : ITemplatesService
    {
        private readonly ITemplatesManager _templateManager;

        public TemplatesService(ITemplatesManager templateManager)
        {
            _templateManager = templateManager;
        }

        public async Task Create(string name, string subjectTemplate, string language, string htmlBodyTemplate, string plainBody, EncodingType? htmlEncoding,
            EncodingType? plainEncoding)
        {
            var template = new DefaultTemplate<Guid>
            {
                Id = Guid.NewGuid(),
                Name = name,
                SubjectTemplate = subjectTemplate,
                HtmlBodyTemplate = htmlBodyTemplate,
                PlainBody = plainBody,
                HtmlBodyEncoding = htmlEncoding != null ? EncodingConverter.CastToEncoding(htmlEncoding.Value) : null,
                PlainBodyEncoding = plainEncoding != null ? EncodingConverter.CastToEncoding(plainEncoding.Value) : null,
                Language = language
            };

            await _templateManager.Create(template);
        }

        public async Task<ITemplate<Guid>> GetById(Guid templateId)
        {
            var template = await _templateManager.GetById(templateId);
            return template;
        }
    }
}
