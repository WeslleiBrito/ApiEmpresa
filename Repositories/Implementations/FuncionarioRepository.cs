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
                .Include(f => f.Endereco)
                .Include(f => f.Profissao)
                .Include(f => f.Profissao!.Setor)
                .FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}
