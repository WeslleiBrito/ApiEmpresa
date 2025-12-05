using ApiEmpresas.Models;

namespace ApiEmpresas.Repositories.Interfaces
{
    public interface ISetorRepository : IRepository<Setor>
    {
        Task<IEnumerable<Setor>> GetByIdsAsync(IEnumerable<Guid> ids);
        Task<bool> ExisteProfissoesAsync(Guid setorId);
        Task<bool> ExisteEmpresasAsync(Guid setorId);
        Task<bool> ExisteNomeAsync(string nome);
    }
}
