using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cactus.Email.Core.Managers
{
    public interface ITemplatesReader<TKey>
    {
        Task<IEnumerable<ITemplate<TKey>>> GetByLanguage(string language);

        Task<ITemplate<TKey>> GetById(TKey id);
    }
}
