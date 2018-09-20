using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cactus.Email.Core.Services
{
    public interface IEmailService<TTemplateKey>
    {
        Task Send(TTemplateKey templateId, string from, IEnumerable<string> recipients);

        Task Send<TBodyModel, TSubjectModel>(TTemplateKey templateId, string from, IEnumerable<string> recipients, TBodyModel bodyModel, TSubjectModel subjectModel);

        Task SendWithBodyModel<TBodyModel>(TTemplateKey templateId, string from, IEnumerable<string> recipients, TBodyModel bodyModel);

        Task SendWithSubjectModel<TSubjectModel>(TTemplateKey templateId, string from, IEnumerable<string> recipients, TSubjectModel subjectModel);
    }
}
