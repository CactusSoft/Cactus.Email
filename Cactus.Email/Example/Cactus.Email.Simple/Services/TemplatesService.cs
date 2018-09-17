using System;
using System.Threading.Tasks;
using Cactus.Email.Core.Repositories;
using Cactus.Email.Core.Senders;
using Cactus.Email.Templates.EntityFraemwork;
using Microsoft.EntityFrameworkCore;

namespace Cactus.Email.Simple.Services
{
    public class TemplatesService : ITemplatesService
    {
        private readonly ITemplatesRepository _templatesRepository;

        public TemplatesService(ITemplatesRepository templatesRepository)
        {
            _templatesRepository = templatesRepository;
        }

        public async Task Create(string name, string subjectTemplate, string language, string bodyTemplate, string plainBody, EncodingType? htmlEncoding,
            EncodingType? plainEncoding)
        {
            await _templatesRepository.CreateAsync(new Template
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
            });
        }

        public async Task<ITemplate> GetById(Guid templateId)
        {
            return await _templatesRepository.GetQuerable().FirstOrDefaultAsync(x => x.Id == templateId);
        }
    }
}
