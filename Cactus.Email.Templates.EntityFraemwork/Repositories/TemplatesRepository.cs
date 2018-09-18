using System.Linq;
using System.Threading.Tasks;
using Cactus.Email.Core.Repositories;
using Cactus.Email.Templates.EntityFraemwork.Database;
using Newtonsoft.Json.Linq;

namespace Cactus.Email.Templates.EntityFraemwork.Repositories
{
    public class TemplatesRepository : ITemplatesRepository
    {
        private readonly TemplatesDbContext _context;

        public TemplatesRepository(TemplatesDbContext context)
        {
            _context = context;
        }

        public IQueryable<ITemplate> GetQuerable()
        {
            return _context.Set<Template>();
        }

        public async Task CreateAsync(ITemplate entity)
        {
            await _context.Set<Template>().AddAsync((Template)entity);
            await _context.SaveChangesAsync();
        }

        public async Task PatchAsync(ITemplate entity, JObject patch)
        {
            _context.Patch(patch, (Template)entity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(ITemplate entity)
        {
            _context.Remove((Template)entity);
            await _context.SaveChangesAsync();
        }
    }
}
