using System.Text.RegularExpressions;

namespace Cactus.Email.Core.Utils
{
    public class SubjectTemplateParser : ISubjectTemplateParser
    {
        private readonly Regex _sendindDateRegex = new Regex("<%>sending-date<%>");
        private readonly Regex _senderEmailRegex = new Regex("<%>sender-email<%>");
        private readonly Regex _recipientsEmailsRegex = new Regex("<%>recipients-email<%>");

        public string Parse(string template, string senderEmail, string recipientsEmails, string createdDateTime)
        {
            template = _sendindDateRegex.Replace(template, createdDateTime);
            template = _senderEmailRegex.Replace(template, senderEmail);
            template = _recipientsEmailsRegex.Replace(template, recipientsEmails);

            return template;
        }
    }
}
