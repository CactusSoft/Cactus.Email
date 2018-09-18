using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Cactus.Email.Core.Repositories
{
    public interface ITemplatesRepository
    {
        Task CreateAsync(ITemplate template);
        Task PatchAsync(ITemplate template, JObject patch);
        Task RemoveAsync(ITemplate template);

        IQueryable<ITemplate> GetQuerable();
    }
}
