using System.Linq;
using System.Threading.Tasks;
using Cactus.Email.Templates.EntityFraemwork.Managers;

namespace Cactus.Email.Templates.EntityFraemwork.Repositories
{
    public interface ITemplatesRepository
    {
        IQueryable<Template> GetQuerable();

        Task CreateAsync(Template entity);
        Task UpdateAsync(Template entity);
        Task RemoveAsync(Template entity);
    }
}
