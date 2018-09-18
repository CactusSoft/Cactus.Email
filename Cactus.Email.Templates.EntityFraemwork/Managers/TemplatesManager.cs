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
            return await _templatesRepository.GetQuerable().Where(x => x.Language == language).ToListAsync();
        }

        public async Task<ITemplate<Guid>> GetById(Guid id)
        {
            return await _templatesRepository.GetQuerable().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Create(ITemplate<Guid> template)
        {
            try
            {
                await _templatesRepository.CreateAsync((Template)template);
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
                template.BodyTemplate = templateUpdates.BodyTemplate;
                template.IsBodyHtml = templateUpdates.IsBodyHtml;
                template.PlainBody = templateUpdates.PlainBody;
                template.Language = templateUpdates.Language;
                template.HtmlBodyEncoding = templateUpdates.HtmlBodyEncoding;
                template.PlainBodyEncoding = templateUpdates.PlainBodyEncoding;

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
    }
}
