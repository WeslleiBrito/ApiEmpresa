using ApiEmpresas.Models;

namespace ApiEmpresas.Repositories.Interfaces
{
    public interface IHabilidadeRepository : IRepository<Habilidade>
    {
        Task<Habilidade?> GetHabilidadeCompletaAsync(Guid id);
        new Task<IEnumerable<Habilidade>> GetAllAsync();
        Task<Habilidade?> GetByIdWithFuncionariosAsync(Guid id);
        Task<List<Habilidade>> GetByIdsAsync(IEnumerable<Guid> ids);

        Task<List<Habilidade>> Add(IEnumerable<Guid> ids);
    }
}