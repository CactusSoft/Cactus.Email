using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using Cactus.Email.Core.Senders;
using Cactus.Email.Smtp.Configurations;
using NUnit.Framework;

namespace Cactus.Email.Smtp.Tests
{
    [TestFixture]
    public class SmtpSenderTests
    {
        [Test]
        public void GenerateMessage_Success()
        {
            //Arrange
            var smtpConfiguration = new SmtpConfiguration("server", 81, "account", "password", 1, true);
            var smtpSender = new SmtpSender(smtpConfiguration);
            var generateMessageMethod = smtpSender.GetType().GetMethod("GenerateMessage", BindingFlags.NonPublic | BindingFlags.Instance);

            var fromEmail = "kirill.pototsky@cactussoft.biz";
            var recipients = new[] { "artem.glushov@gmail.com", "veronika.potapkina@gmail.com" };
            var webPageInfo = new EmailContentInfo("Test subject", "Hi", "Hello", "ru", Encoding.UTF8, Encoding.ASCII);
            string actualPlainText;

            //Expect

            //Act
            var generatedMessage = (MailMessage)generateMessageMethod.Invoke(smtpSender, new object[] { fromEmail, recipients, webPageInfo, "" });

            //Assert
            Assert.IsNotNull(generatedMessage);

            Assert.AreEqual(fromEmail, generatedMessage.From.Address);
            Assert.AreEqual(webPageInfo.Subject, generatedMessage.Subject);
            Assert.AreEqual(webPageInfo.HtmlBody, generatedMessage.Body);
            Assert.IsTrue(generatedMessage.IsBodyHtml);

            Assert.AreEqual(Encoding.UTF8.EncodingName, generatedMessage.BodyEncoding.EncodingName);

            Assert.AreEqual(recipients.Length, generatedMessage.To.Count);
            Assert.AreEqual(recipients[0], generatedMessage.To[0].Address);
            Assert.AreEqual(recipients[1], generatedMessage.To[1].Address);

            Assert.AreEqual(1, generatedMessage.AlternateViews.Count);
            Assert.AreEqual("text/plain", generatedMessage.AlternateViews[0].ContentType.MediaType);
            using (var reader = new StreamReader(generatedMessage.AlternateViews[0].ContentStream))
            {
                actualPlainText = reader.ReadToEnd();
            }
            Assert.AreEqual(webPageInfo.PlainBody, actualPlainText);

            //Verify
        }

        [Test]
        public void GetNewSmtpClient_Success()
        {
            //Arrange
            var smtpConfiguration = new SmtpConfiguration("smt.gmail.com", 587, "email@gmail.com", "1234", 100000, true);
            var smtpSender = new SmtpSender(smtpConfiguration);
            var getNewSmtpClientMethod = smtpSender.GetType().GetMethod("GetNewSmtpClient", BindingFlags.NonPublic | BindingFlags.Instance);
            NetworkCredential actualCredentials;
            
            //Expect

            //Act
            var smtpClient = (SmtpClient)getNewSmtpClientMethod.Invoke(smtpSender, null);
            
            //Assert
            Assert.IsNotNull(smtpClient);
            Assert.AreEqual(smtpConfiguration.SmtpServer ,smtpClient.Host);
            Assert.IsFalse(smtpClient.UseDefaultCredentials);
            actualCredentials = (NetworkCredential)smtpClient.Credentials; 
            Assert.AreEqual(smtpConfiguration.SmtpAccount, actualCredentials.UserName);
            Assert.AreEqual(smtpConfiguration.SmtpAccountPassword, actualCredentials.Password);

            Assert.AreEqual(smtpConfiguration.SmtpServerPort, smtpClient.Port);
            Assert.AreEqual(smtpConfiguration.EnableSsl, smtpClient.EnableSsl);
            Assert.AreEqual(smtpConfiguration.SmtpServerTimeout, smtpClient.Timeout);

            //Verify
        }
    }
}
