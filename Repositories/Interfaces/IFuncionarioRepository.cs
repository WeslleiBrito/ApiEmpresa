using ApiEmpresas.Models;

namespace ApiEmpresas.Repositories.Interfaces
{
    public interface IFuncionarioRepository : IRepository<Funcionario>
    {
        Task<Funcionario?> GetFuncionarioCompletoAsync(Guid id);
        Task<bool> ExisteFuncionarioPorEmpresaAsync(Guid empresaId);

        Task<bool> ExisteCpfAsync(string cpf);
        Task<IEnumerable<Funcionario>> GetAllCompletoAsync();
    }
}
