using System.Globalization;
using System.Threading.Tasks;

namespace Cactus.FluentEmail.Source.Core.Interfaces
{
    public interface ITemplatesWriter
    {
        Task Create(ITemplate template);
        Task Update(string name, CultureInfo language, ITemplate templateUpdates);
        Task Remove(string name, CultureInfo language);
    }
}
