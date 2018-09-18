using System.Threading.Tasks;
using Cactus.Email.Simple.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cactus.Email.Simple.Controllers
{
    [Route("emails")]
    public class EmailController : Controller
    {
        private readonly IEmailSender _emailSender;

        public EmailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpPost("email")]
        public async Task<IActionResult> Send([FromBody]SendEmailFrom form)
        {
            await _emailSender.Send(form.TemplateId, form.FromAddress, form.Recipients);
            return NoContent();
        }
    }
}
