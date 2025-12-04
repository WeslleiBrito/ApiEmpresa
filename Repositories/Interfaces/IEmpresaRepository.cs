using ApiEmpresas.Models;

namespace ApiEmpresas.Repositories.Interfaces
{
    public interface IEmpresaRepository : IRepository<Empresa>
    {
        Task<Empresa?> GetEmpresaCompletaAsync(Guid id);
        new Task<IEnumerable<Empresa>> GetAllAsync();

        Task<IEnumerable<EmpresaSetor>> GetEmpresaSetoresAsync(Guid empresaId);
        void RemoveEmpresaSetores(IEnumerable<EmpresaSetor> setores);

    }
}
