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
                        .ThenInclude(s => s.Funcionarios)
                            .ThenInclude(fs => fs.Funcionario)
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(e => e.Id == id);
        }


        public new async Task<IEnumerable<Empresa>> GetAllAsync()
        {
            return await _dbSet
                .Include(e => e.Endereco)
                .Include(e => e.Setores)
                    .ThenInclude(es => es.Setor)
                        .ThenInclude(s => s.Funcionarios)
                            .ThenInclude(fs => fs.Funcionario)
                .ToListAsync();
        }


        public async Task<IEnumerable<EmpresaSetor>> GetEmpresaSetoresAsync(Guid empresaId)
        {
            return await _context.EmpresaSetores
                .Where(es => es.EmpresaId == empresaId)
                .ToListAsync();
        }


        public async Task<Empresa?> GetByIdWithFullDataAsync(Guid id)
        {
            return await _context.Empresas
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
                .AsNoTracking()
                .Include(e => e.Setores)
                .ThenInclude(es => es.Setor)
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public Task AddSetorAsync(Guid empresaId, Guid setorId)
        {
            _context.EmpresaSetores.Add(new EmpresaSetor
            {
                EmpresaId = empresaId,
                SetorId = setorId
            });

            return Task.CompletedTask;
        }

        public async Task RemoveSetorAsync(Guid empresaId, Guid setorId)
        {
            var vinculo = await _context.EmpresaSetores.FindAsync(empresaId, setorId);
            if (vinculo != null)
                _context.EmpresaSetores.Remove(vinculo);
        }


        public async Task AddFuncionarioAsync(Guid empresaId, Guid setorId, Guid funcionarioId, decimal salario)
        {
            var vinculo = new FuncionarioSetor
            {
                EmpresaId = empresaId,
                SetorId = setorId,
                FuncionarioId = funcionarioId,
                Salario = salario
            };

            await _context.FuncionarioSetores.AddAsync(vinculo);
        }
        public async Task RemoveFuncionarioAsync(Guid empresaId, Guid setorId, Guid funcionarioId)
        {
            var vinculo = await _context.FuncionarioSetores.FindAsync(
                funcionarioId, empresaId, setorId);

            if (vinculo != null)
            {
                _context.FuncionarioSetores.Remove(vinculo);
            }
        }

    }
}
