using ApiEmpresas.Data;
using ApiEmpresas.Models;
using ApiEmpresas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiEmpresas.Repositories.Implementations
{
    public class HabilidadeRepository : Repository<Habilidade>, IHabilidadeRepository
    {
        public HabilidadeRepository(AppDbContext context) : base(context)
        {
        }

        public new async Task<IEnumerable<Habilidade>> GetAllAsync()
        {
            return await _dbSet
                .Include(h => h.Funcionarios)
                .ThenInclude(fh => fh.Funcionario)
                .ToListAsync();
        }

        public async Task<Habilidade?> GetByIdWithFuncionariosAsync(Guid id)
        {
            return await _dbSet
                .Include(h => h.Funcionarios)
                .ThenInclude(fh => fh.Funcionario)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<Habilidade?> GetHabilidadeCompletaAsync(Guid id)
        {
            return await _dbSet
                .Include(h => h.Funcionarios)
                .ThenInclude(fh => fh.Funcionario)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<List<Habilidade>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            return await _context.Habilidades
                .Where(h => ids.Contains(h.Id))
                .ToListAsync();
        }

    }
}