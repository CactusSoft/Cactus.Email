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

        public TemplateController(ITemplatesService templatesServices)
        {
            _templatesServices = templatesServices;
        }

        [HttpPost("template")]
        public async Task<IActionResult> Create([FromBody]CreateEmailTemplateForm form)
        {
            await _templatesServices.Create(form.Name, form.Subject, form.Language, form.HtmlBody, form.PlainBody,
                    form.HtmlBodyEncoding, form.PlainBodyEncoding);
            return NoContent();
        }
    }
}
