using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cactus.Email.Core.Managers;
using Cactus.Email.Core.Senders;
using Cactus.Email.Core.Utils;

namespace Cactus.Email.Core.Services
{
    public class DefaultEmailService<TTemplateKey> : IEmailService<TTemplateKey>
    {
        private readonly ISender _sender;
        private readonly ITemplatesReader<TTemplateKey> _templatesService;
        private readonly ITemplateRenderer _templateRenderer;

        public DefaultEmailService(ISender sender, ITemplatesReader<TTemplateKey> templateService, ITemplateRenderer templateRenderer)
        {
            _sender = sender;
            _templatesService = templateService;
            _templateRenderer = templateRenderer;
        }

        public virtual async Task Send(TTemplateKey templateId, string from, IEnumerable<string> recipients)
        {
            var collectionRecipients = recipients.ToList();

            var template = await _templatesService.GetById(templateId);
            var htmlBody = !string.IsNullOrEmpty(template.HtmlBodyTemplate) ? _templateRenderer.Render(template.HtmlBodyTemplate) : null;
            var subject = _templateRenderer.Render(template.SubjectTemplate);
            var emailContentInfo = new EmailContentInfo(subject, htmlBody, template.PlainBody, template.Language, template.HtmlBodyEncoding, template.PlainBodyEncoding);

            await _sender.Send(from, collectionRecipients, emailContentInfo);
        }

        public virtual async Task Send<TBodyModel, TSubjectModel>(TTemplateKey templateId, string from, IEnumerable<string> recipients, TBodyModel bodyModel, TSubjectModel subjectModel)
        {
            var collectionRecipients = recipients.ToList();

            var template = await _templatesService.GetById(templateId);
            var htmlBody = !string.IsNullOrEmpty(template.HtmlBodyTemplate) ? _templateRenderer.Render(template.HtmlBodyTemplate, bodyModel) : null;
            var subject = _templateRenderer.Render(template.SubjectTemplate, subjectModel);
            var emailContentInfo = new EmailContentInfo(subject, htmlBody, template.PlainBody, template.Language, template.HtmlBodyEncoding, template.PlainBodyEncoding);

            await _sender.Send(from, collectionRecipients, emailContentInfo);
        }

        public virtual async Task SendWithBodyModel<TBodyModel>(TTemplateKey templateId, string from, IEnumerable<string> recipients, TBodyModel bodyModel)
        {
            var collectionRecipients = recipients.ToList();

            var template = await _templatesService.GetById(templateId);
            var htmlBody = !string.IsNullOrEmpty(template.HtmlBodyTemplate) ? _templateRenderer.Render(template.HtmlBodyTemplate, bodyModel) : null;
            var subject = _templateRenderer.Render(template.SubjectTemplate);
            var emailContentInfo = new EmailContentInfo(subject, htmlBody, template.PlainBody, template.Language, template.HtmlBodyEncoding, template.PlainBodyEncoding);

            await _sender.Send(from, collectionRecipients, emailContentInfo);
        }

        public virtual async Task SendWithSubjectModel<TSubjectModel>(TTemplateKey templateId, string from, IEnumerable<string> recipients, TSubjectModel subjectModel)
        {
            var collectionRecipients = recipients.ToList();

            var template = await _templatesService.GetById(templateId);
            var htmlBody = !string.IsNullOrEmpty(template.HtmlBodyTemplate) ? _templateRenderer.Render(template.HtmlBodyTemplate) : null;
            var subject = _templateRenderer.Render(template.SubjectTemplate, subjectModel);
            var emailContentInfo = new EmailContentInfo(subject, htmlBody, template.PlainBody, template.Language, template.HtmlBodyEncoding, template.PlainBodyEncoding);

            await _sender.Send(from, collectionRecipients, emailContentInfo);
        }
    }
}
