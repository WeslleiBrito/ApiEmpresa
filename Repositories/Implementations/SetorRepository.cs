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

        public async Task<bool> ExisteEmpresasAsync(Guid setorId)
        {
            return await _context.EmpresaSetores.AnyAsync(es => es.SetorId == setorId);
        }

        public async Task<bool> ExisteNomeAsync(string nome)
        {
            return await _dbSet.AnyAsync(s => s.Nome.ToLower() == nome.ToLower());
        }

        public async Task<bool> ExisteNomeParaOutroIdAsync(string nome, Guid id)
        {
            return await _dbSet.AnyAsync(s => s.Nome.ToLower() == nome.ToLower() && s.Id != id);
        }
    }
}
