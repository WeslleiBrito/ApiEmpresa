using ApiEmpresas.Data;
using ApiEmpresas.Models;
using ApiEmpresas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiEmpresas.Repositories.Implementations
{
    public class FuncionarioSetorRepository : IFuncionarioSetorRepository
    {
        private readonly AppDbContext _context;

        public FuncionarioSetorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(FuncionarioSetor vinculo)
        {
            await _context.FuncionarioSetores.AddAsync(vinculo);
        }

        public async Task RemoveAsync(Guid funcionarioId, Guid empresaId, Guid setorId)
        {
            var entity = await _context.FuncionarioSetores.FindAsync(
                funcionarioId, empresaId, setorId);

            if (entity != null)
                _context.FuncionarioSetores.Remove(entity);
        }

        public async Task<bool> ExistsAsync(Guid funcionarioId, Guid empresaId, Guid setorId)
        {
            return await _context.FuncionarioSetores.AnyAsync(fs =>
                fs.FuncionarioId == funcionarioId &&
                fs.EmpresaId == empresaId &&
                fs.SetorId == setorId);
        }
    }

}