using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cactus.Email.Core.Managers;
using Cactus.Email.Templates.EntityFraemwork.Logging;
using Cactus.Email.Templates.EntityFraemwork.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Cactus.Email.Templates.EntityFraemwork.Managers
{
    public class TemplatesManager : ITemplatesManager
    {
        private readonly ITemplatesRepository _templatesRepository;
        private readonly ILog _logger = LogProvider.GetLogger(typeof(TemplatesManager));

        public TemplatesManager(ITemplatesRepository templatesRepository)
        {
            _templatesRepository = templatesRepository;
        }

        public async Task<IEnumerable<ITemplate<Guid>>> GetByLanguage(string language)
        {
            var entity = await _templatesRepository.GetQuerable().Where(x => x.Language == language).ToListAsync();
            return entity.Select(CastToDefaultTemplate);
        }

        public async Task<ITemplate<Guid>> GetById(Guid id)
        {
            var entity = await _templatesRepository.GetQuerable().FirstOrDefaultAsync(x => x.Id == id);
            return CastToDefaultTemplate(entity);
        }

        public async Task Create(ITemplate<Guid> template)
        {
            try
            {
                var entity = new Template
                {
                    Id = template.Id,
                    Name = template.Name,
                    SubjectTemplate = template.SubjectTemplate,
                    HtmlBodyTemplate = template.HtmlBodyTemplate,
                    PlainBody = template.PlainBody,
                    Language = template.Language,
                    CreatedDateTime = DateTime.UtcNow
                };

                if (template.HtmlBodyEncoding != null)
                {
                    entity.HtmlBodyEncoding = EncodingConverter.CastToEncodingType(template.HtmlBodyEncoding);
                }

                if (template.PlainBodyEncoding != null)
                {
                    entity.PlainBodyEncoding = EncodingConverter.CastToEncodingType(template.PlainBodyEncoding);
                }

                await _templatesRepository.CreateAsync(entity);
            }
            catch (DbUpdateException ex)
            {
                _logger.Error(ex, "Failed to create template");
                throw;
            }
        }

        public async Task Update(Guid id, ITemplate<Guid> templateUpdates)
        {
            try
            {
                var template = await _templatesRepository.GetQuerable().FirstOrDefaultAsync(x => x.Id == id);
                if (template == null)
                {
                    var erroMessage = $"Failed to update template because wasn't fond template with id {id}";
                    _logger.Error(erroMessage);
                    throw new ArgumentException(erroMessage);
                }
                template.Name = templateUpdates.Name;
                template.SubjectTemplate = templateUpdates.SubjectTemplate;
                template.HtmlBodyTemplate = templateUpdates.HtmlBodyTemplate;
                template.PlainBody = templateUpdates.PlainBody;
                template.Language = templateUpdates.Language;

                if (template.HtmlBodyEncoding != null)
                {
                    template.HtmlBodyEncoding = EncodingConverter.CastToEncodingType(templateUpdates.HtmlBodyEncoding);
                }

                if (templateUpdates.PlainBodyEncoding != null)
                {
                    template.PlainBodyEncoding = EncodingConverter.CastToEncodingType(templateUpdates.PlainBodyEncoding);
                }

                await _templatesRepository.UpdateAsync(template);
            }
            catch (DbUpdateException ex)
            {
                _logger.Error(ex, "Failed to update template");
                throw;
            }
        }

        public async Task Remove(Guid id)
        {
            try
            {
                var template = await _templatesRepository.GetQuerable().FirstOrDefaultAsync(x => x.Id == id);
                if (template == null)
                {
                    var erroMessage = $"Failed to delete template because wasn't fond template with id {id}";
                    _logger.Error(erroMessage);
                    throw new ArgumentException(erroMessage);
                }
                await _templatesRepository.RemoveAsync(template);
            }
            catch (DbUpdateException ex)
            {
                _logger.Error(ex, "Failed to delete template");
                throw;
            }
        }

        private DefaultTemplate<Guid> CastToDefaultTemplate(Template template)
        {
            return new DefaultTemplate<Guid>
            {
                Id = template.Id,
                Name = template.Name,
                SubjectTemplate = template.SubjectTemplate,
                HtmlBodyTemplate = template.HtmlBodyTemplate,
                PlainBody = template.PlainBody,
                Language = template.Language,
                HtmlBodyEncoding = template.HtmlBodyEncoding != null ? EncodingConverter.CastToEncoding(template.HtmlBodyEncoding.Value) : null,
                PlainBodyEncoding = template.PlainBodyEncoding != null ? EncodingConverter.CastToEncoding(template.PlainBodyEncoding.Value) : null
            };
        }
    }
}
