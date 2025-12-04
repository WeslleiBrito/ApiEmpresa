using ApiEmpresas.Models;

namespace ApiEmpresas.Repositories.Interfaces
{
    public interface IFuncionarioRepository : IRepository<Funcionario>
    {
        Task<Funcionario?> GetFuncionarioCompletoAsync(Guid id);
        Task<bool> ExisteFuncionarioPorSetorAsync(Guid setorId);
        Task<bool> ExisteFuncionarioPorEmpresaAsync(Guid empresaId);


    }
}
