using ApiEmpresas.Models;

namespace ApiEmpresas.Repositories.Interfaces
{
    public interface ISetorRepository : IRepository<Setor>
    {
        Task<IEnumerable<Setor>> GetByIdsAsync(IEnumerable<Guid> ids);
        Task<bool> ExisteEmpresasAsync(Guid setorId);
        Task<bool> ExisteNomeAsync(string nome);
        Task<bool> ExisteNomeParaOutroIdAsync(string nome, Guid id);

    }
}
