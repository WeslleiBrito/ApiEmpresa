using ApiEmpresas.Models;

namespace ApiEmpresas.Repositories.Interfaces
{
    public interface IFuncionarioSetorRepository
    {
        Task AddAsync(FuncionarioSetor vinculo);
        Task RemoveAsync(Guid funcionarioId, Guid empresaId, Guid setorId);
        Task<bool> ExistsAsync(Guid funcionarioId, Guid empresaId, Guid setorId);
    }
}