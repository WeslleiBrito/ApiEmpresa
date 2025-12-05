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
            return await _context.Empresas
                .Include(e => e.Setores)
                    .ThenInclude(es => es.Setor)
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(e => e.Id == id);
        }


        public async Task<Empresa?> GetByCnpjAsync(string cnpj)
        {
            return await _dbSet
                .Include(e => e.Endereco)
                .Include(e => e.Setores)
                    .ThenInclude(es => es.Setor)
                .FirstOrDefaultAsync(e => e.Cnpj == cnpj);
        }

        public new async Task<IEnumerable<Empresa>> GetAllAsync()
        {
            return await _dbSet
                .Include(e => e.Endereco)
                .Include(e => e.Setores)
                    .ThenInclude(es => es.Setor)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmpresaSetor>> GetEmpresaSetoresAsync(Guid empresaId)
        {
            return await _context.EmpresaSetores
                .Where(es => es.EmpresaId == empresaId)
                .ToListAsync();
        }

        public void RemoveEmpresaSetores(IEnumerable<EmpresaSetor> setores)
        {
            _context.EmpresaSetores.RemoveRange(setores);
        }

        public async Task<Empresa?> GetByIdWithFullDataAsync(Guid id)
        {
            return await _context.Empresas
            .AsNoTracking()
                .Include(e => e.Setores)
                    .ThenInclude(es => es.Setor)
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(e => e.Id == id);
        }


        public Task<bool> CnpjExisteParaOutraEmpresaAsync(string cnpj, Guid empresaId)
        {
            return _context.Empresas
                .AnyAsync(e => e.Cnpj == cnpj && e.Id != empresaId);
        }

        public async Task<Empresa?> GetByIdWithFullDataAsNoTrackingAsync(Guid id)
        {
            return await _context.Empresas
                .AsNoTracking() // Importante: Ignora o cache para trazer os nomes dos setores
                .Include(e => e.Setores)
                    .ThenInclude(es => es.Setor)
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddEmpresaSetoresAsync(IEnumerable<EmpresaSetor> setores)
        {
            await _context.EmpresaSetores.AddRangeAsync(setores);
        }
    }
}
