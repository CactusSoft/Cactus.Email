using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cactus.Email.Core.Senders;
using Cactus.Email.Core.Utils;
using Cactus.Email.Simple.Services;
using Microsoft.EntityFrameworkCore.Internal;

namespace Cactus.Email.Simple
{
    public class EmailSender : IEmailSender
    {
        private readonly ISender _sender;
        private readonly ITemplatesService _templatesService;
        private readonly ITemplateRenderer _templateRenderer;
        private readonly ISubjectTemplateParser _subjectTemplateParser;

        public EmailSender(ISender sender, ITemplatesService templateService, ITemplateRenderer templateRenderer, ISubjectTemplateParser subjectTemplateParser)
        {
            _sender = sender;
            _templatesService = templateService;
            _templateRenderer = templateRenderer;
            _subjectTemplateParser = subjectTemplateParser;
        }

        public async Task Send(Guid templateid, string from, IEnumerable<string> recipients)
        {
            var collectionRecipients = recipients.ToList();

            var template = await _templatesService.GetById(templateid);
            var htmlBody = _templateRenderer.Render(template.BodyTemplate);
            var subject = _subjectTemplateParser.Parse(template.SubjectTemplate, from, collectionRecipients.Join(","), DateTime.UtcNow.ToString());
            var pageInfo = new WebPageInfo(subject, htmlBody, template.PlainBody, template.IsBodyHtml, template.Language, template.HtmlBodyEncoding, template.PlainBodyEncoding);

            await _sender.Send(from, collectionRecipients, pageInfo);
        }

        public async Task Send<TModel>(Guid templateid, string from, IEnumerable<string> recipients, TModel model)
        {
            var collectionRecipients = recipients.ToList();

            var template = await _templatesService.GetById(templateid);
            var htmlBody = _templateRenderer.Render(template.BodyTemplate, model);
            var subject = _subjectTemplateParser.Parse(template.SubjectTemplate, from, collectionRecipients.Join(","), DateTime.UtcNow.ToString());
            var pageInfo = new WebPageInfo(subject, htmlBody, template.PlainBody, template.IsBodyHtml, template.Language, template.HtmlBodyEncoding, template.PlainBodyEncoding);

            await _sender.Send(from, collectionRecipients, pageInfo);
        }
    }
}
