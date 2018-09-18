using System;
using System.Threading;
using System.Threading.Tasks;
using Cactus.Email.Core.Senders;
using Cactus.Email.Templates.EntityFraemwork.Database;
using Cactus.Email.Templates.EntityFraemwork.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using NUnit.Framework;

namespace Cactus.Email.Templates.EntityFraemwork.Tests.Repositories
{
    [TestFixture]
    public class TemplatesRepositoryTests
    {
        [Test]
        public async Task CreateAsync_Success()
        {
            //Arrange
            var context = new Mock<TemplatesDbContext>();
            var dbSetTemplate = new Mock<DbSet<Template>>();
            var templatesRepository = new TemplatesRepository(context.Object);
            var template = new Template
            {
                Id = Guid.NewGuid(),
                Name = "Test template",
                SubjectTemplate = "Subject of test template",
                BodyTemplate = "Body of test template",
                IsBodyHtml = true,
                PlainBody = "Plain body of test template",
                Language = "ru",
                HtmlBodyEncoding = EncodingType.UTF8,
                PlainBodyEncoding = EncodingType.ASCII,
                CreatedDateTime = DateTime.UtcNow
            };
            Template actualTemplate = null;

            //Expect
            context.Setup(x => x.Set<Template>())
                .Returns(dbSetTemplate.Object);

            dbSetTemplate.Setup(x => x.AddAsync(It.IsNotNull<Template>(), CancellationToken.None))
                .Callback<Template>(t =>
                {
                    actualTemplate = t;
                })
                .Returns(Task.FromResult(It.IsNotNull<EntityEntry<Template>>()));

            context.Setup(x => x.SaveChangesAsync(CancellationToken.None))
                .Returns(Task.FromResult(1));

            //Act
            await templatesRepository.CreateAsync(template);

            //Assert
            Assert.IsNotNull(actualTemplate);
            Assert.AreEqual(template.Id, actualTemplate.Id);
            Assert.AreEqual(template.Name, actualTemplate.Name);
            Assert.AreEqual(template.SubjectTemplate, actualTemplate.SubjectTemplate);
            Assert.AreEqual(template.BodyTemplate, actualTemplate.BodyTemplate);
            Assert.AreEqual(template.IsBodyHtml, actualTemplate.IsBodyHtml);
            Assert.AreEqual(template.PlainBody, actualTemplate.PlainBody);
            Assert.AreEqual(template.Language, actualTemplate.Language);
            Assert.AreEqual(template.HtmlBodyEncoding, actualTemplate.HtmlBodyEncoding);
            Assert.AreEqual(template.PlainBodyEncoding, actualTemplate.PlainBodyEncoding);
            Assert.AreEqual(template.CreatedDateTime, actualTemplate.CreatedDateTime);
            
            //Verify
        }
    }
}
