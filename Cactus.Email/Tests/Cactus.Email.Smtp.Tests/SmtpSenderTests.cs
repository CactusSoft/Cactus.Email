using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Cactus.Email.Core.Senders;
using Cactus.Email.Smtp.Configurations;
using Moq;
using NUnit.Framework;

namespace Cactus.Email.Smtp.Tests
{
    [TestFixture]
    public class SmtpSenderTests
    {
        [Test]
        public async Task Send_Success()
        {
            //Arrange
            var smtpClient = new Mock<SmtpClient>();
            var smtpConfiguration = new SmtpConfiguration("server", 81, "account", "password", 1, true);
            var smtpSender = new SmtpSender(smtpConfiguration);

            MailMessage actualMailMessage = null;
            var fromEmail = "kirill.pototsky@cactussoft.biz";
            var recipients = new[] { "artem.glushov@gmail.com", "veronika.potapkina@gmail.com" };
            var webPageIngo = new WebPageInfo("Test subject", "Hi", "Hello", false, "ru", EncodingType.UTF8, EncodingType.UTF8);

            //Expect
            smtpClient.Setup(x => x.SendMailAsync(It.IsAny<MailMessage>()))
                .Callback<MailMessage>(message =>
                {
                    actualMailMessage = message;
                })
                .Returns(Task.CompletedTask)
                .Verifiable();

            //Act
            await smtpSender.Send(fromEmail, recipients, webPageIngo);

            //Assert
            Assert.IsNotNull(actualMailMessage);

            Assert.AreEqual(recipients.Length, actualMailMessage.To.Count);
            Assert.AreEqual(recipients[0], actualMailMessage.To[0].Address);
            Assert.AreEqual(recipients[1], actualMailMessage.To[1].Address);

            Assert.AreEqual(fromEmail, actualMailMessage.From.Address);
            Assert.AreEqual(webPageIngo.Subject, actualMailMessage.Subject);
            Assert.AreEqual(webPageIngo.Body, actualMailMessage.Body);
            Assert.AreEqual(webPageIngo.IsBodyHtml, actualMailMessage.IsBodyHtml);

            Assert.AreEqual(Encoding.UTF8, actualMailMessage.BodyEncoding);

            Assert.AreEqual(1, actualMailMessage.AlternateViews.Count);
            Assert.AreEqual("text/plain", actualMailMessage.AlternateViews[0].ContentType.MediaType);
            Assert.AreEqual(Encoding.UTF8, actualMailMessage.AlternateViews[0].TransferEncoding);
            
            //Verify
            smtpClient.VerifyAll();
        }
    }
}
