using ApiEmpresas.Models;

namespace ApiEmpresas.Repositories.Interfaces
{
    public interface IFuncionarioRepository : IRepository<Funcionario>
    {
        Task<Funcionario?> GetFuncionarioCompletoAsync(Guid id);
    }
}
