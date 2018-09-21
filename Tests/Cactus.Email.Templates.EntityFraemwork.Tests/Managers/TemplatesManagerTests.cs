using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cactus.Email.Core.Managers;
using Cactus.Email.Templates.EntityFraemwork.Database;
using Cactus.Email.Templates.EntityFraemwork.Managers;
using Cactus.Email.Templates.EntityFraemwork.Repositories;
using Moq;
using NUnit.Framework;

namespace Cactus.Email.Templates.EntityFraemwork.Tests.Managers
{
    [TestFixture]
    public class TemplatesManagerTests
    {
        [Test]
        public async Task GetByLanguage_Success()
        {
            //Arrange
            var templatesRepository = new Mock<ITemplatesRepository>();
            var templatesManager = new TemplatesManager(templatesRepository.Object);

            var searchingLanguage = "en";
            var entities = new List<Template>
            {
                new Template
                {
                    Id = Guid.NewGuid(),
                    Name = "tests template 1",
                    SubjectTemplate = "tests subject 1",
                    HtmlBodyTemplate = "Hi my friend :)",
                    PlainBody = "test plain",
                    Language = searchingLanguage,
                    HtmlBodyEncoding = EncodingType.ASCII,
                    PlainBodyEncoding = EncodingType.UTF32,
                    CreatedDateTime = DateTime.UtcNow
                },
                new Template
                {
                    Id = Guid.NewGuid(),
                    Name = "tests template 3",
                    SubjectTemplate = "tests subject 2",
                    HtmlBodyTemplate = "Hi my friend :)",
                    PlainBody = "test plain",
                    Language = searchingLanguage,
                    HtmlBodyEncoding = EncodingType.ASCII,
                    PlainBodyEncoding = EncodingType.UTF32,
                    CreatedDateTime = DateTime.UtcNow
                },
                new Template
                {
                    Id = Guid.NewGuid(),
                    Name = "tests template 2",
                    SubjectTemplate = "tests subject 2",
                    HtmlBodyTemplate = "Hi my friend :)",
                    PlainBody = "test plain",
                    Language = "ru",
                    HtmlBodyEncoding = EncodingType.ASCII,
                    PlainBodyEncoding = EncodingType.UTF32,
                    CreatedDateTime = DateTime.UtcNow
                }
            };

            //Expect
            templatesRepository
                .Setup(x => x.GetQuerable())
                .Returns(new AsyncEnumerable<Template>(entities));

            //Act
            var filteredTemplates = (await templatesManager.GetByLanguage(searchingLanguage))?.ToList();

            //Assert
            Assert.IsNotNull(filteredTemplates);
            Assert.AreEqual(2, filteredTemplates.Count);

            Assert.AreEqual(entities[0].Id, filteredTemplates[0].Id);
            Assert.AreEqual(entities[0].Name, filteredTemplates[0].Name);
            Assert.AreEqual(entities[0].SubjectTemplate, filteredTemplates[0].SubjectTemplate);
            Assert.AreEqual(entities[0].HtmlBodyTemplate, filteredTemplates[0].HtmlBodyTemplate);
            Assert.AreEqual(entities[0].PlainBody, filteredTemplates[0].PlainBody);
            Assert.AreEqual(entities[0].Language, filteredTemplates[0].Language);
            Assert.AreEqual(Encoding.ASCII, filteredTemplates[0].HtmlBodyEncoding);
            Assert.AreEqual(Encoding.UTF32, filteredTemplates[0].PlainBodyEncoding);

            Assert.AreEqual(entities[1].Id, filteredTemplates[1].Id);
            Assert.AreEqual(entities[1].Name, filteredTemplates[1].Name);
            Assert.AreEqual(entities[1].SubjectTemplate, filteredTemplates[1].SubjectTemplate);
            Assert.AreEqual(entities[1].HtmlBodyTemplate, filteredTemplates[1].HtmlBodyTemplate);
            Assert.AreEqual(entities[1].PlainBody, filteredTemplates[1].PlainBody);
            Assert.AreEqual(entities[1].Language, filteredTemplates[1].Language);
            Assert.AreEqual(Encoding.ASCII, filteredTemplates[1].HtmlBodyEncoding);
            Assert.AreEqual(Encoding.UTF32, filteredTemplates[1].PlainBodyEncoding);

            //Verify
            templatesRepository.VerifyAll();
        }

        [Test]
        public async Task GetById_Success()
        {
            //Arrange
            var templatesRepository = new Mock<ITemplatesRepository>();
            var templatesManager = new TemplatesManager(templatesRepository.Object);

            var searchingId = Guid.NewGuid();
            var entities = new List<Template>
            {
                new Template
                {
                    Id = Guid.NewGuid(),
                    Name = "tests template 1",
                    SubjectTemplate = "tests subject 1",
                    HtmlBodyTemplate = "Hi my friend :)",
                    PlainBody = "test plain",
                    Language = "en",
                    HtmlBodyEncoding = EncodingType.ASCII,
                    PlainBodyEncoding = EncodingType.UTF32,
                    CreatedDateTime = DateTime.UtcNow
                },
                new Template
                {
                    Id = searchingId,
                    Name = "tests template 2",
                    SubjectTemplate = "tests subject 2",
                    HtmlBodyTemplate = "Hi my friend :)",
                    PlainBody = "test plain",
                    Language = "ru",
                    HtmlBodyEncoding = EncodingType.ASCII,
                    PlainBodyEncoding = EncodingType.UTF32,
                    CreatedDateTime = DateTime.UtcNow
                }
            };

            //Expect
            templatesRepository
                .Setup(x => x.GetQuerable())
                .Returns(new AsyncEnumerable<Template>(entities));

            //Act
            var filteredTemplate = await templatesManager.GetById(searchingId);

            //Assert
            Assert.IsNotNull(filteredTemplate);

            Assert.AreEqual(entities[1].Id, filteredTemplate.Id);
            Assert.AreEqual(entities[1].Name, filteredTemplate.Name);
            Assert.AreEqual(entities[1].SubjectTemplate, filteredTemplate.SubjectTemplate);
            Assert.AreEqual(entities[1].HtmlBodyTemplate, filteredTemplate.HtmlBodyTemplate);
            Assert.AreEqual(entities[1].PlainBody, filteredTemplate.PlainBody);
            Assert.AreEqual(entities[1].Language, filteredTemplate.Language);
            Assert.AreEqual(Encoding.ASCII, filteredTemplate.HtmlBodyEncoding);
            Assert.AreEqual(Encoding.UTF32, filteredTemplate.PlainBodyEncoding);

            //Verify
            templatesRepository.VerifyAll();
        }

        [Test]
        public async Task Create_Success()
        {
            //Arrange
            var templatesRepository = new Mock<ITemplatesRepository>();
            var templatesManager = new TemplatesManager(templatesRepository.Object);

            var expectedTemplate = new DefaultTemplate<Guid>
            {
                Id = Guid.NewGuid(),
                Name = "test template",
                SubjectTemplate = "test subject",
                HtmlBodyTemplate = "Hi, it's your email",
                PlainBody = "test plain",
                Language = "ru",
                HtmlBodyEncoding = Encoding.ASCII,
                PlainBodyEncoding = Encoding.UTF8
            };
            Template actualTemplate = null;

            //Expect
            templatesRepository
                .Setup(x => x.CreateAsync(It.IsNotNull<Template>()))
                .Callback<Template>(template =>
                {
                    actualTemplate = template;
                })
                .Returns(Task.CompletedTask);

            //Act
            await templatesManager.Create(expectedTemplate);

            //Assert
            Assert.IsNotNull(actualTemplate);
            Assert.AreEqual(expectedTemplate.Id, actualTemplate.Id);
            Assert.AreEqual(expectedTemplate.Name, actualTemplate.Name);
            Assert.AreEqual(expectedTemplate.SubjectTemplate, actualTemplate.SubjectTemplate);
            Assert.AreEqual(expectedTemplate.HtmlBodyTemplate, actualTemplate.HtmlBodyTemplate);
            Assert.AreEqual(expectedTemplate.PlainBody, actualTemplate.PlainBody);
            Assert.AreEqual(expectedTemplate.Language, actualTemplate.Language);

            //Verify
            templatesRepository.VerifyAll();
        }

        [Test]
        public async Task Update_Success()
        {
            //Arrange
            var templatesRepository = new Mock<ITemplatesRepository>();
            var templatesManager = new TemplatesManager(templatesRepository.Object);

            var searchingTemplateId = Guid.NewGuid();
            var entities = new List<Template>
            {
                new Template
                {
                    Id = Guid.NewGuid(),
                    Name = "tests template 1",
                    SubjectTemplate = "tests subject 1",
                    HtmlBodyTemplate = "Hi my friend :)",
                    PlainBody = "test plain",
                    Language = "en",
                    HtmlBodyEncoding = EncodingType.ASCII,
                    PlainBodyEncoding = EncodingType.UTF32,
                    CreatedDateTime = DateTime.UtcNow
                },
                new Template
                {
                    Id = searchingTemplateId,
                    Name = "tests template 2",
                    SubjectTemplate = "tests subject 2",
                    HtmlBodyTemplate = "Hi my friend :)",
                    PlainBody = "test plain",
                    Language = "ru",
                    HtmlBodyEncoding = EncodingType.ASCII,
                    PlainBodyEncoding = EncodingType.UTF32,
                    CreatedDateTime = DateTime.UtcNow
                }
            };

            var templatesUpdates = new DefaultTemplate<Guid>
            {
                Id = Guid.NewGuid(),
                Name = "test template",
                SubjectTemplate = "test subject",
                HtmlBodyTemplate = "Hi, it's your email",
                PlainBody = "test plain",
                Language = "en",
                HtmlBodyEncoding = Encoding.Unicode,
                PlainBodyEncoding = Encoding.UTF8
            };
            Template actualTemplateUpdates = null;

            //Expect
            templatesRepository
                .Setup(x => x.UpdateAsync(It.IsNotNull<Template>()))
                .Callback<Template>(template =>
                {
                    actualTemplateUpdates = template;
                })
                .Returns(Task.CompletedTask);

            templatesRepository
                .Setup(x => x.GetQuerable())
                .Returns(new AsyncEnumerable<Template>(entities));

            //Act
            await templatesManager.Update(searchingTemplateId, templatesUpdates);

            //Assert
            Assert.IsNotNull(actualTemplateUpdates);
            Assert.AreEqual(searchingTemplateId, actualTemplateUpdates.Id);
            Assert.AreEqual(templatesUpdates.Name, actualTemplateUpdates.Name);
            Assert.AreEqual(templatesUpdates.SubjectTemplate, actualTemplateUpdates.SubjectTemplate);
            Assert.AreEqual(templatesUpdates.HtmlBodyTemplate, actualTemplateUpdates.HtmlBodyTemplate);
            Assert.AreEqual(templatesUpdates.PlainBody, actualTemplateUpdates.PlainBody);
            Assert.AreEqual(templatesUpdates.Language, actualTemplateUpdates.Language);
            Assert.AreEqual(entities[1].CreatedDateTime, actualTemplateUpdates.CreatedDateTime);

            //Verify
            templatesRepository.VerifyAll();
        }

        [Test]
        public async Task Remove_Success()
        {
            //Arrange
            var templatesRepository = new Mock<ITemplatesRepository>();
            var templatesManager = new TemplatesManager(templatesRepository.Object);

            var searchingTemplateId = Guid.NewGuid();
            var entities = new List<Template>
            {
                new Template
                {
                    Id = Guid.NewGuid(),
                    Name = "tests template 1",
                    SubjectTemplate = "tests subject 1",
                    HtmlBodyTemplate = "Hi my friend :)",
                    PlainBody = "test plain",
                    Language = "en",
                    HtmlBodyEncoding = EncodingType.ASCII,
                    PlainBodyEncoding = EncodingType.UTF32,
                    CreatedDateTime = DateTime.UtcNow
                },
                new Template
                {
                    Id = searchingTemplateId,
                    Name = "tests template 2",
                    SubjectTemplate = "tests subject 2",
                    HtmlBodyTemplate = "Hi my friend :)",
                    PlainBody = "test plain",
                    Language = "ru",
                    HtmlBodyEncoding = EncodingType.ASCII,
                    PlainBodyEncoding = EncodingType.UTF32,
                    CreatedDateTime = DateTime.UtcNow
                }
            };
            Template removedTemplate = null;

            //Expect
            templatesRepository
                .Setup(x => x.GetQuerable())
                .Returns(new AsyncEnumerable<Template>(entities));

            templatesRepository
                .Setup(x => x.RemoveAsync(It.IsNotNull<Template>()))
                .Callback<Template>(template =>
                {
                    removedTemplate = template;
                })
                .Returns(Task.CompletedTask);

            //Act
            await templatesManager.Remove(searchingTemplateId);

            //Assert
            Assert.IsNotNull(removedTemplate);
            Assert.AreEqual(searchingTemplateId, removedTemplate.Id);
            Assert.AreEqual(entities[1].Name, removedTemplate.Name);
            Assert.AreEqual(entities[1].SubjectTemplate, removedTemplate.SubjectTemplate);
            Assert.AreEqual(entities[1].HtmlBodyTemplate, removedTemplate.HtmlBodyTemplate);
            Assert.AreEqual(entities[1].PlainBody, removedTemplate.PlainBody);
            Assert.AreEqual(entities[1].Language, removedTemplate.Language);
            Assert.AreEqual(entities[1].HtmlBodyEncoding, removedTemplate.HtmlBodyEncoding);
            Assert.AreEqual(entities[1].PlainBodyEncoding, removedTemplate.PlainBodyEncoding);
            Assert.AreEqual(entities[1].CreatedDateTime, removedTemplate.CreatedDateTime);

            //Verify
            templatesRepository.VerifyAll();
        }
    }
}
