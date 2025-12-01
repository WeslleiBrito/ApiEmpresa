using ApiEmpresas.Data;
using ApiEmpresas.Models;
using ApiEmpresas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiEmpresas.Repositories.Implementations
{
    public class EmpresaRepository : Repository<Empresa>, IEmpresaRepository
    {
        public EmpresaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Empresa?> GetEmpresaCompletaAsync(Guid id)
        {
            return await _dbSet
                .Include(e => e.Endereco)
                .Include(e => e.Setores)
                .ThenInclude(s => s.Setor)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public new async Task<IEnumerable<Empresa>> GetAllAsync()
{
        return await _dbSet
            .Include(e => e.Endereco)
            .Include(e => e.Setores)
                .ThenInclude(es => es.Setor)
            .ToListAsync();
        }
        
    }
}
