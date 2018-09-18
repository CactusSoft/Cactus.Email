using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cactus.Email.Simple.Models;
using Cactus.Email.Simple.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cactus.Email.Simple.Controllers
{
    [Route("templates")]
    public class TemplateController : Controller
    {
        private readonly ITemplatesService _templatesServices;
        private readonly IEmailSender _emailSender;

        public TemplateController(ITemplatesService templatesServices, IEmailSender emailSender)
        {
            _templatesServices = templatesServices;
            _emailSender = emailSender;
        }

        [HttpPost("template")]
        public async Task<IActionResult> Create([FromBody]CreateEmailTemplateForm form)
        {
            await _templatesServices.Create(form.Name, form.Subject, form.Language, form.HtmlBody, form.PlainBody,
                    form.HtmlBodyEncoding, form.PlainBodyEncoding);
            return NoContent();
        }

        [HttpPost("test/{templateId}")]
        public async Task<IActionResult> Send([FromRoute]Guid templateId, [FromBody]string from, [FromBody]List<string> recipients)
        {
            recipients = new List<string> { "kirill.pototsky@cactussoft.biz" };
            await _emailSender.Send(templateId, from, recipients, new TestModel { Name = "Kiiiiiiiirill" });
            return NoContent();
        }
    }
}
