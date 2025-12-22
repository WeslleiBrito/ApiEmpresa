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
            return await _context.Funcionarios
                .AsNoTracking()
                .Include(f => f.Endereco)
                .Include(f => f.Habilidades)
                    .ThenInclude(fh => fh.Habilidade)
                .Include(f => f.FuncionarioSetorVinculo)
                    .ThenInclude(fs => fs.Empresa)
                .Include(f => f.FuncionarioSetorVinculo)
                    .ThenInclude(fs => fs.Setor)
                .FirstOrDefaultAsync(f => f.Id == id);
        }


        public async Task<bool> ExisteCpfAsync(string cpf)
        {
            return await _context.Funcionarios.AnyAsync(f => f.Cpf == cpf);
        }

        public async Task<IEnumerable<Funcionario>> GetAllCompletoAsync()
        {
            return await _context.Funcionarios
                .AsNoTracking()
                .Include(f => f.Endereco)
                .Include(f => f.Habilidades)
                    .ThenInclude(fh => fh.Habilidade)
                .Include(f => f.FuncionarioSetorVinculo)
                    .ThenInclude(fs => fs.Empresa)
                .Include(f => f.FuncionarioSetorVinculo)
                    .ThenInclude(fs => fs.Setor)
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

        public async Task<IEnumerable<Funcionario>> FindAsync(IEnumerable<Guid> ids)
        {
            return await _dbSet
                .Where(f => ids.Contains(f.Id))
                .ToListAsync();
        }
    }
}
