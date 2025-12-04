using ApiEmpresas.Data;
using ApiEmpresas.Models;
using ApiEmpresas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiEmpresas.Repositories.Implementations
{
    public class SetorRepository : Repository<Setor>, ISetorRepository
    {
        public SetorRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Setor>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            return await _dbSet
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();
        }
    }
}
