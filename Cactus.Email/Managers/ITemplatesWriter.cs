using System.Threading.Tasks;

namespace Cactus.Email.Core.Managers
{
    public interface ITemplatesWriter<TKey>
    {
        Task Create(ITemplate<TKey> template);
        Task Update(TKey id, ITemplate<TKey> templateUpdates);
        Task Remove(TKey id);
    }
}
