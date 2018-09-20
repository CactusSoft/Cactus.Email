using System;
using System.Threading.Tasks;
using Cactus.Email.Core.Services;
using Cactus.Email.Simple.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cactus.Email.Simple.Controllers
{
    [Route("emails")]
    public class EmailController : Controller
    {
        private readonly IEmailService<Guid> _emailService;

        public EmailController(IEmailService<Guid> emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("email")]
        public async Task<IActionResult> Send([FromBody]SendEmailFrom form)
        {
            await _emailService.SendWithBodyModel(form.TemplateId, form.FromAddress, form.Recipients, new TestModel() {Name = "Kiiiiiriiiil))))"});
            return NoContent();
        }
    }
}
