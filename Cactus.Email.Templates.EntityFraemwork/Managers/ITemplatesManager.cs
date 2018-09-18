using System;
using Cactus.Email.Core.Managers;

namespace Cactus.Email.Templates.EntityFraemwork.Managers
{
    public interface ITemplatesManager : ITemplatesReader<Guid>, ITemplatesWriter<Guid>
    {
    }
}
