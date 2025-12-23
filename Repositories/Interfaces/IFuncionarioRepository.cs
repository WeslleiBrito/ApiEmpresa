using ApiEmpresas.Models;

namespace ApiEmpresas.Repositories.Interfaces
{
    public interface IFuncionarioRepository : IRepository<Funcionario>
    {
        Task<Funcionario?> GetFuncionarioCompletoAsync(Guid id);

        Task<bool> ExisteCpfAsync(string cpf);
        Task<IEnumerable<Funcionario>> GetAllCompletoAsync();
        Task AddFuncionarioHabilidadesAsync(IEnumerable<FuncionarioHabilidade> vinculos);
        void RemoveFuncionarioHabilidades(IEnumerable<FuncionarioHabilidade> vinculos);

        Task<IEnumerable<Funcionario>> FindAsync(IEnumerable<Guid> ids);
    }
}
