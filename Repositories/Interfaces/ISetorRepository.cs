using ApiEmpresas.Models;

namespace ApiEmpresas.Repositories.Interfaces
{
    public interface ISetorRepository : IRepository<Setor>
    {
        Task<IEnumerable<Setor>> GetByIdsAsync(IEnumerable<Guid> ids);
    }
}
