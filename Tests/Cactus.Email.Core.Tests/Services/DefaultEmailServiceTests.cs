using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cactus.Email.Core.Managers;
using Cactus.Email.Core.Senders;
using Cactus.Email.Core.Services;
using Cactus.Email.Core.Utils;
using Moq;
using NUnit.Framework;

namespace Cactus.Email.Core.Tests.Services
{
    [TestFixture]
    public class DefaultEmailServiceTests
    {
        private readonly List<DefaultTemplate<Guid>> _testTemplates = new List<DefaultTemplate<Guid>>
        {
            new DefaultTemplate<Guid>
            {
                Id = Guid.NewGuid(),
                Name = "tests template 1",
                SubjectTemplate = "tests subject 1",
                HtmlBodyTemplate = "Hi my friend :)",
                PlainBody = "test plain",
                Language = "en",
                HtmlBodyEncoding = Encoding.ASCII,
                PlainBodyEncoding = Encoding.UTF32
            },
            new DefaultTemplate<Guid>
            {
                Id = Guid.NewGuid(),
                Name = "tests template 2",
                SubjectTemplate = "tests subject 2",
                HtmlBodyTemplate = "Hi my friend :)",
                PlainBody = "test plain",
                Language = "ru",
                HtmlBodyEncoding = Encoding.ASCII,
                PlainBodyEncoding = Encoding.UTF32
            }
        };

        [Test]
        public async Task SendWithoutModels_Success()
        {
            //Arrange
            var sender = new Mock<ISender>();
            var templatesReader = new Mock<ITemplatesReader<Guid>>();
            var templateRenderer = new Mock<ITemplateRenderer>();
            var defaultEmailService = new DefaultEmailService<Guid>(sender.Object, templatesReader.Object, templateRenderer.Object);

            var expectedTemplateId = _testTemplates[1].Id;
            var expectedFrom = "kirill.pototsky@cactussoft.biz";
            var expectedRecipients = new List<string> { "vladimir@gmail.com", "alina@gmail.com" };

            string actualFrom = null;
            List<string> actualRecipients = null;
            IEmailContentInfo actualEmailContentInfo = null;

            //Expect
            templatesReader
                .Setup(x => x.GetById(It.Is<Guid>(id => id == expectedTemplateId)))
                .Returns(Task.FromResult((ITemplate<Guid>)_testTemplates.FirstOrDefault(x => x.Id == expectedTemplateId)));

            templateRenderer
                .Setup(x => x.Render(It.IsNotNull<string>()))
                .Returns((string x) => x);

            sender
                .Setup(x => x.Send(It.IsNotNull<string>(), It.IsNotNull<IEnumerable<string>>(), It.IsNotNull<IEmailContentInfo>(), null))
                    .Callback<string, IEnumerable<string>, IEmailContentInfo, string>((from, recipients, emailContentInfoInfo, displayName) =>
                    {
                        actualFrom = from;
                        actualRecipients = recipients.ToList();
                        actualEmailContentInfo = emailContentInfoInfo;
                    })
                .Returns(Task.CompletedTask);

            //Act
            await defaultEmailService.Send(expectedTemplateId, expectedFrom, expectedRecipients);

            //Assert
            Assert.IsNotNull(actualFrom);
            Assert.IsNotNull(actualRecipients);
            Assert.IsNotNull(actualEmailContentInfo);

            Assert.AreEqual(expectedFrom, actualFrom);
            Assert.AreEqual(expectedRecipients.Count, actualRecipients.Count);
            Assert.AreEqual(expectedRecipients[0], actualRecipients[0]);
            Assert.AreEqual(expectedRecipients[1], actualRecipients[1]);

            Assert.AreEqual(_testTemplates[1].HtmlBodyTemplate, actualEmailContentInfo.HtmlBody);
            Assert.AreEqual(_testTemplates[1].HtmlBodyEncoding, actualEmailContentInfo.HtmlBodyEncoding);
            Assert.AreEqual(_testTemplates[1].PlainBody, actualEmailContentInfo.PlainBody);
            Assert.AreEqual(_testTemplates[1].PlainBodyEncoding, actualEmailContentInfo.PlainBodyEncoding);
            Assert.AreEqual(_testTemplates[1].SubjectTemplate, actualEmailContentInfo.Subject);
            Assert.AreEqual(_testTemplates[1].Language, actualEmailContentInfo.Language);

            //Verify
            templatesReader.VerifyAll();
            templateRenderer.VerifyAll();
            sender.VerifyAll();
        }

        [Test]
        public async Task SendWithSubjectAndBodyModel_Success()
        {
            //Arrange
            var sender = new Mock<ISender>();
            var templatesReader = new Mock<ITemplatesReader<Guid>>();
            var templateRenderer = new Mock<ITemplateRenderer>();
            var defaultEmailService = new DefaultEmailService<Guid>(sender.Object, templatesReader.Object, templateRenderer.Object);

            var expectedTemplateId = _testTemplates[1].Id;
            var expectedFrom = "kirill.pototsky@cactussoft.biz";
            var expectedRecipients = new List<string> { "vladimir@gmail.com", "alina@gmail.com" };
            var bodyModel = new DefaultTemplate<Guid> { Name = "Kirill" };
            var subjectModel = "hi!)";

            string actualFrom = null;
            List<string> actualRecipients = null;
            IEmailContentInfo actualEmailContentInfo = null;
            DefaultTemplate<Guid> actualBodyModel = null;
            string actualSubjectModel = null;

            //Expect
            templatesReader
                .Setup(x => x.GetById(It.Is<Guid>(id => id == expectedTemplateId)))
                .Returns(Task.FromResult((ITemplate<Guid>)_testTemplates.FirstOrDefault(x => x.Id == expectedTemplateId)));

            templateRenderer
                .Setup(x => x.Render(It.IsNotNull<string>(), It.IsNotNull<DefaultTemplate<Guid>>()))
                .Callback<string, DefaultTemplate<Guid>>((template, model) =>
                {
                    actualBodyModel = model;
                })
                .Returns((string x, DefaultTemplate<Guid> m) => x);

            templateRenderer
                .Setup(x => x.Render(It.IsNotNull<string>(), It.IsNotNull<string>()))
                .Callback<string, string>((template, model) =>
                {
                    actualSubjectModel = model;
                })
                .Returns((string x, string m) => x);

            sender
                .Setup(x => x.Send(It.IsNotNull<string>(), It.IsNotNull<IEnumerable<string>>(), It.IsNotNull<IEmailContentInfo>(), null))
                    .Callback<string, IEnumerable<string>, IEmailContentInfo, string>((from, recipients, emailContentInfoInfo, displayName) =>
                    {
                        actualFrom = from;
                        actualRecipients = recipients.ToList();
                        actualEmailContentInfo = emailContentInfoInfo;
                    })
                .Returns(Task.CompletedTask);


            //Act
            await defaultEmailService.Send(expectedTemplateId, expectedFrom, expectedRecipients, bodyModel, subjectModel);

            //Assert
            Assert.IsNotNull(actualFrom);
            Assert.IsNotNull(actualRecipients);
            Assert.IsNotNull(actualEmailContentInfo);
            Assert.IsNotNull(actualBodyModel);
            Assert.IsNotNull(actualSubjectModel);

            Assert.AreEqual(expectedFrom, actualFrom);
            Assert.AreEqual(expectedRecipients.Count, actualRecipients.Count);
            Assert.AreEqual(expectedRecipients[0], actualRecipients[0]);
            Assert.AreEqual(expectedRecipients[1], actualRecipients[1]);

            Assert.AreEqual(_testTemplates[1].HtmlBodyTemplate, actualEmailContentInfo.HtmlBody);
            Assert.AreEqual(_testTemplates[1].HtmlBodyEncoding, actualEmailContentInfo.HtmlBodyEncoding);
            Assert.AreEqual(_testTemplates[1].PlainBody, actualEmailContentInfo.PlainBody);
            Assert.AreEqual(_testTemplates[1].PlainBodyEncoding, actualEmailContentInfo.PlainBodyEncoding);
            Assert.AreEqual(_testTemplates[1].SubjectTemplate, actualEmailContentInfo.Subject);
            Assert.AreEqual(_testTemplates[1].Language, actualEmailContentInfo.Language);

            Assert.AreEqual(bodyModel.Name, actualBodyModel.Name);
            Assert.AreEqual(subjectModel, actualSubjectModel);

            //Verify
            templatesReader.VerifyAll();
            templateRenderer.VerifyAll();
            sender.VerifyAll();
        }

        [Test]
        public async Task SendWithBodyModel_Success()
        {
            //Arrange
            var sender = new Mock<ISender>();
            var templatesReader = new Mock<ITemplatesReader<Guid>>();
            var templateRenderer = new Mock<ITemplateRenderer>();
            var defaultEmailService = new DefaultEmailService<Guid>(sender.Object, templatesReader.Object, templateRenderer.Object);

            var expectedTemplateId = _testTemplates[1].Id;
            var expectedFrom = "kirill.pototsky@cactussoft.biz";
            var expectedRecipients = new List<string> { "vladimir@gmail.com", "alina@gmail.com" };
            var bodyModel = new DefaultTemplate<Guid> { Name = "Kirill" };

            string actualFrom = null;
            List<string> actualRecipients = null;
            IEmailContentInfo actualEmailContentInfo = null;
            DefaultTemplate<Guid> actualBodyModel = null;

            //Expect
            templatesReader
                .Setup(x => x.GetById(It.Is<Guid>(id => id == expectedTemplateId)))
                .Returns(Task.FromResult((ITemplate<Guid>)_testTemplates.FirstOrDefault(x => x.Id == expectedTemplateId)));

            templateRenderer
                .Setup(x => x.Render(It.IsNotNull<string>(), It.IsNotNull<DefaultTemplate<Guid>>()))
                .Callback<string, DefaultTemplate<Guid>>((template, model) =>
                {
                    actualBodyModel = model;
                })
                .Returns((string x, DefaultTemplate<Guid> m) => x);

            templateRenderer
                .Setup(x => x.Render(It.IsNotNull<string>()))
                .Returns((string x) => x);

            sender
                .Setup(x => x.Send(It.IsNotNull<string>(), It.IsNotNull<IEnumerable<string>>(), It.IsNotNull<IEmailContentInfo>(), null))
                    .Callback<string, IEnumerable<string>, IEmailContentInfo, string>((from, recipients, emailContentInfoInfo, displayName) =>
                    {
                        actualFrom = from;
                        actualRecipients = recipients.ToList();
                        actualEmailContentInfo = emailContentInfoInfo;
                    })
                .Returns(Task.CompletedTask);

            //Act
            await defaultEmailService.SendWithBodyModel(expectedTemplateId, expectedFrom, expectedRecipients, bodyModel);

            //Assert
            Assert.IsNotNull(actualFrom);
            Assert.IsNotNull(actualRecipients);
            Assert.IsNotNull(actualEmailContentInfo);
            Assert.IsNotNull(actualBodyModel);

            Assert.AreEqual(expectedFrom, actualFrom);
            Assert.AreEqual(expectedRecipients.Count, actualRecipients.Count);
            Assert.AreEqual(expectedRecipients[0], actualRecipients[0]);
            Assert.AreEqual(expectedRecipients[1], actualRecipients[1]);

            Assert.AreEqual(_testTemplates[1].HtmlBodyTemplate, actualEmailContentInfo.HtmlBody);
            Assert.AreEqual(_testTemplates[1].HtmlBodyEncoding, actualEmailContentInfo.HtmlBodyEncoding);
            Assert.AreEqual(_testTemplates[1].PlainBody, actualEmailContentInfo.PlainBody);
            Assert.AreEqual(_testTemplates[1].PlainBodyEncoding, actualEmailContentInfo.PlainBodyEncoding);
            Assert.AreEqual(_testTemplates[1].SubjectTemplate, actualEmailContentInfo.Subject);
            Assert.AreEqual(_testTemplates[1].Language, actualEmailContentInfo.Language);

            Assert.AreEqual(bodyModel.Name, actualBodyModel.Name);

            //Verify
            templatesReader.VerifyAll();
            templateRenderer.VerifyAll();
            sender.VerifyAll();
        }

        [Test]
        public async Task SendWithSubjectModel_Success()
        {
            //Arrange
            var sender = new Mock<ISender>();
            var templatesReader = new Mock<ITemplatesReader<Guid>>();
            var templateRenderer = new Mock<ITemplateRenderer>();
            var defaultEmailService = new DefaultEmailService<Guid>(sender.Object, templatesReader.Object, templateRenderer.Object);

            var expectedTemplateId = _testTemplates[1].Id;
            var expectedFrom = "kirill.pototsky@cactussoft.biz";
            var expectedRecipients = new List<string> { "vladimir@gmail.com", "alina@gmail.com" };
            var subjectModel = "hi!)";

            string actualFrom = null;
            List<string> actualRecipients = null;
            IEmailContentInfo actualEmailContentInfo = null;
            string actualSubjectModel = null;

            //Expect
            templatesReader
                .Setup(x => x.GetById(It.Is<Guid>(id => id == expectedTemplateId)))
                .Returns(Task.FromResult((ITemplate<Guid>)_testTemplates.FirstOrDefault(x => x.Id == expectedTemplateId)));

            templateRenderer
                .Setup(x => x.Render(It.IsNotNull<string>()))
                .Returns((string x) => x);

            templateRenderer
                .Setup(x => x.Render(It.IsNotNull<string>(), It.IsNotNull<string>()))
                .Callback<string, string>((template, model) =>
                {
                    actualSubjectModel = model;
                })
                .Returns((string x, string m) => x);

            sender
                .Setup(x => x.Send(It.IsNotNull<string>(), It.IsNotNull<IEnumerable<string>>(), It.IsNotNull<IEmailContentInfo>(), null))
                    .Callback<string, IEnumerable<string>, IEmailContentInfo, string>((from, recipients, emailContentInfoInfo, displayName) =>
                    {
                        actualFrom = from;
                        actualRecipients = recipients.ToList();
                        actualEmailContentInfo = emailContentInfoInfo;
                    })
                .Returns(Task.CompletedTask);


            //Act
            await defaultEmailService.SendWithSubjectModel(expectedTemplateId, expectedFrom, expectedRecipients, subjectModel);

            //Assert
            Assert.IsNotNull(actualFrom);
            Assert.IsNotNull(actualRecipients);
            Assert.IsNotNull(actualEmailContentInfo);
            Assert.IsNotNull(actualSubjectModel);

            Assert.AreEqual(expectedFrom, actualFrom);
            Assert.AreEqual(expectedRecipients.Count, actualRecipients.Count);
            Assert.AreEqual(expectedRecipients[0], actualRecipients[0]);
            Assert.AreEqual(expectedRecipients[1], actualRecipients[1]);

            Assert.AreEqual(_testTemplates[1].HtmlBodyTemplate, actualEmailContentInfo.HtmlBody);
            Assert.AreEqual(_testTemplates[1].HtmlBodyEncoding, actualEmailContentInfo.HtmlBodyEncoding);
            Assert.AreEqual(_testTemplates[1].PlainBody, actualEmailContentInfo.PlainBody);
            Assert.AreEqual(_testTemplates[1].PlainBodyEncoding, actualEmailContentInfo.PlainBodyEncoding);
            Assert.AreEqual(_testTemplates[1].SubjectTemplate, actualEmailContentInfo.Subject);
            Assert.AreEqual(_testTemplates[1].Language, actualEmailContentInfo.Language);

            Assert.AreEqual(subjectModel, actualSubjectModel);

            //Verify
            templatesReader.VerifyAll();
            templateRenderer.VerifyAll();
            sender.VerifyAll();
        }
    }
}
