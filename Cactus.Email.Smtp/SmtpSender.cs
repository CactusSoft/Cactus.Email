using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
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

        public async Task Send(string fromEmail, IEnumerable<string> recipients, IWebPageInfo webPageInfo, string displayName = null)
        {
            try
            {
                var message = GenerateMessage(fromEmail, recipients, webPageInfo, displayName);

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

        public async Task Send(string fromEmail, string recipient, IWebPageInfo template, string displayName = null)
        {
            await Send(fromEmail, new[] { recipient }, template);
        }

        private MailMessage GenerateMessage(string fromEmail, IEnumerable<string> recipients, IWebPageInfo webPageInfo, string displayName = null)
        {
            var collectionRecipients = recipients.ToList();

            var message = new MailMessage
            {
                From = string.IsNullOrEmpty(displayName) ? new MailAddress(fromEmail) : new MailAddress(fromEmail, displayName),
                Subject = webPageInfo.Subject,
                Body = webPageInfo.Body,
                IsBodyHtml = webPageInfo.IsBodyHtml
            };

            if (webPageInfo.HtmlBodyEncoding != null)
            {
                message.BodyEncoding = CastToEncoding(webPageInfo.HtmlBodyEncoding.Value);
            }

            if (!string.IsNullOrEmpty(webPageInfo.PlainBody))
            {
                if (webPageInfo.PlainBodyEncoding != null)
                {
                    message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(webPageInfo.PlainBody,
                        CastToEncoding(webPageInfo.PlainBodyEncoding.Value), "text/plain"));
                }
                else
                {
                    message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(webPageInfo.PlainBody,
                        new ContentType { MediaType = "text/plain" }));
                }
            }

            foreach (var recipient in collectionRecipients.Distinct())
            {
                message.To.Add(recipient);
            }

            return message;
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

        private Encoding CastToEncoding(EncodingType encodingType)
        {
            switch (encodingType)
            {
                case EncodingType.Default: return Encoding.Default;
                case EncodingType.Unicode: return Encoding.Unicode;
                case EncodingType.UTF8: return Encoding.UTF8;
                case EncodingType.ASCII: return Encoding.ASCII;
                case EncodingType.UTF7: return Encoding.UTF7;
                case EncodingType.UTF32: return Encoding.UTF32;
                case EncodingType.BigEndianUnicode: return Encoding.BigEndianUnicode;
                default:
                    var errorMessage = "Couldn't defined type of encoding";
                    _logger.Error(errorMessage);
                    throw new ArgumentOutOfRangeException(nameof(encodingType), errorMessage);
            }
        }
    }
}
