using ApiEmpresas.Data;
using ApiEmpresas.Models;
using ApiEmpresas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiEmpresas.Repositories.Implementations
{
    public class ProfissaoRepository : Repository<Profissao>, IProfissaoRepository
    {
        public ProfissaoRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Profissao>> GetAllAsync()
        {
            return await _dbSet
                .Include(p => p.Setor)
                .ToListAsync();
        }

        public override async Task<Profissao?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(p => p.Setor)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
