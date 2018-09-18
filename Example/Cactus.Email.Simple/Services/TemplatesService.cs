using System;
using System.Threading.Tasks;
using Cactus.Email.Core.Senders;
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

        public async Task Create(string name, string subjectTemplate, string language, string bodyTemplate, string plainBody, EncodingType? htmlEncoding,
            EncodingType? plainEncoding)
        {
            var template = new Template
            {
                Id = Guid.NewGuid(),
                Name = name,
                SubjectTemplate = subjectTemplate,
                BodyTemplate = bodyTemplate,
                PlainBody = plainBody,
                HtmlBodyEncoding = htmlEncoding,
                PlainBodyEncoding = plainEncoding,
                Language = language,
                CreatedDateTime = DateTime.UtcNow
            };

            await _templateManager.Create(template);
        }

        public async Task<Template> GetById(Guid templateId)
        {
            var template = await _templateManager.GetById(templateId);
            return (Template) template;
        }
    }
}
