using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using Cactus.Email.Core.Senders;
using Cactus.Email.Smtp.Configurations;
using Cactus.Email.Smtp.Logging;

namespace Cactus.Email.Smtp
{
    public class SmtpSender : ISender
    {
        private readonly ISmtpConfiguration _smtpConfiguration;
        private readonly ILog _logger = LogProvider.GetLogger(typeof(SmtpSender));

        public SmtpSender(ISmtpConfiguration smtpConfiguration)
        {
            _smtpConfiguration = smtpConfiguration;
        }

        public async Task Send(string fromEmail, IEnumerable<string> recipients, IEmailContentInfo emailContentInfo, string displayName = null)
        {
            try
            {
                var message = GenerateMessage(fromEmail, recipients, emailContentInfo, displayName);

                using (var smtpClient = GetNewSmtpClient())
                {
                    await smtpClient.SendMailAsync(message);
                }
                _logger.Info("Message was send on email.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Couldn't send message on email");
                throw;
            }
        }

        public async Task Send(string fromEmail, string recipient, IEmailContentInfo emailContentInfo, string displayName = null)
        {
            await Send(fromEmail, new[] { recipient }, emailContentInfo, displayName);
        }

        private MailMessage GenerateMessage(string fromEmail, IEnumerable<string> recipients, IEmailContentInfo emailContentInfo, string displayName = null)
        {
            var collectionRecipients = recipients.ToList();

            ValidateMessageParameters(fromEmail, collectionRecipients, emailContentInfo);

            var message = new MailMessage
            {
                From = string.IsNullOrEmpty(displayName) ? new MailAddress(fromEmail) : new MailAddress(fromEmail, displayName)
            };

            if (!string.IsNullOrEmpty(emailContentInfo.Subject))
            {
                message.Subject = emailContentInfo.Subject;
            }

            if (!string.IsNullOrEmpty(emailContentInfo.HtmlBody))
            {
                message.Body = emailContentInfo.HtmlBody;
                message.IsBodyHtml = true;
                if (emailContentInfo.HtmlBodyEncoding != null)
                {
                    message.BodyEncoding = emailContentInfo.HtmlBodyEncoding;
                }
            }

            if (!string.IsNullOrEmpty(emailContentInfo.PlainBody))
            {
                if (emailContentInfo.PlainBodyEncoding != null)
                {
                    message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(emailContentInfo.PlainBody, emailContentInfo.PlainBodyEncoding, "text/plain"));
                }
                else
                {
                    message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(emailContentInfo.PlainBody,
                        new ContentType { MediaType = "text/plain" }));
                }
            }

            foreach (var recipient in collectionRecipients.Distinct())
            {
                message.To.Add(recipient);
            }

            return message;
        }

        private void ValidateMessageParameters(string fromEmail, ICollection<string> recipients, IEmailContentInfo emailContentInfo)
        {
            string errorMessage;
            if (string.IsNullOrEmpty(fromEmail))
            {
                errorMessage = "Failed to generate email message because was not set sender";
                _logger.Error(errorMessage);
                throw new ArgumentException(errorMessage, nameof(emailContentInfo));
            }

            if (recipients.Count <= 0 || recipients.All(string.IsNullOrEmpty))
            {
                errorMessage = "Failed to generate email message because was not set recepient";
                _logger.Error(errorMessage);
                throw new ArgumentException(errorMessage, nameof(emailContentInfo));
            }

            if (string.IsNullOrEmpty(emailContentInfo.HtmlBody) && string.IsNullOrEmpty(emailContentInfo.PlainBody))
            {
                errorMessage = "Failed to generate email message because to need plain body or html body";
                _logger.Error(errorMessage);
                throw new ArgumentException(errorMessage, nameof(emailContentInfo));
            }
        }

        private SmtpClient GetNewSmtpClient()
        {
            var smtpClient = new SmtpClient
            {
                Host = _smtpConfiguration.SmtpServer,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpConfiguration.SmtpAccount, _smtpConfiguration.SmtpAccountPassword)
            };

            if (_smtpConfiguration.SmtpServerPort != null) smtpClient.Port = _smtpConfiguration.SmtpServerPort.Value;
            if (_smtpConfiguration.EnableSsl != null) smtpClient.EnableSsl = _smtpConfiguration.EnableSsl.Value;
            if (_smtpConfiguration.SmtpServerTimeout != null) smtpClient.Timeout = _smtpConfiguration.SmtpServerTimeout.Value;

            return smtpClient;
        }
    }
}
