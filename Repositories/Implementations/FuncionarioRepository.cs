using ApiEmpresas.Data;
using ApiEmpresas.Models;
using ApiEmpresas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiEmpresas.Repositories.Implementations
{
    public class FuncionarioRepository : Repository<Funcionario>, IFuncionarioRepository
    {
        
        public FuncionarioRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Funcionario?> GetFuncionarioCompletoAsync(Guid id)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(f => f.Endereco)
                .Include(f => f.Profissao)
                .Include(f => f.Empresa)
                .Include(f => f.Setores)
                .ThenInclude(fs => fs.Setor)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<bool> ExisteFuncionarioPorEmpresaAsync(Guid empresaId)
        {
            return await _context.Funcionarios.AnyAsync(f => f.EmpresaId == empresaId);
        }

        public async Task<bool> ExisteCpfAsync(string cpf)
        {
            return await _context.Funcionarios.AnyAsync(f => f.Cpf == cpf);
        }

        public async Task<IEnumerable<Funcionario>> GetAllCompletoAsync()
        {
            return await _context.Funcionarios
                .AsNoTracking() // Boa prática para leitura (melhora performance)
                .Include(f => f.Empresa)      // Traz a Empresa
                .Include(f => f.Profissao)    // Traz a Profissão
                .Include(f => f.Setores)      // Traz a tabela de meio (FuncionarioSetor)
                    .ThenInclude(fs => fs.Setor) // <-- ESSA LINHA É CRÍTICA!
                .ToListAsync();
        }

        public async Task AddFuncionarioHabilidadesAsync(IEnumerable<FuncionarioHabilidade> vinculos)
        {
            await _context.FuncionarioHabilidades.AddRangeAsync(vinculos);
        }

        public void RemoveFuncionarioHabilidades(IEnumerable<FuncionarioHabilidade> vinculos)
        {
            _context.FuncionarioHabilidades.RemoveRange(vinculos);
        }
    }
}
