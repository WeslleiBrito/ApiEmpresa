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
                .ToListAsync();
        }

        public override async Task<Profissao?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> ExistsByNomeAsync(string nome)
        {
            return await _dbSet.AnyAsync(p => p.Nome == nome);
        }
    }
}
