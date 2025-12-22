using ApiEmpresas.Models;

namespace ApiEmpresas.Repositories.Interfaces
{
    public interface IEmpresaRepository : IRepository<Empresa>
    {
        Task<Empresa?> GetEmpresaCompletaAsync(Guid id);
        new Task<IEnumerable<Empresa>> GetAllAsync();

        Task<IEnumerable<EmpresaSetor>> GetEmpresaSetoresAsync(Guid empresaId);
        Task<Empresa?> GetByIdWithFullDataAsync(Guid id);
        Task<bool> CnpjExisteParaOutraEmpresaAsync(string cnpj, Guid empresaId);
        Task<Empresa?> GetByIdWithFullDataAsNoTrackingAsync(Guid id);
        Task AddSetorAsync(Guid empresaId, Guid setorId);
        Task RemoveSetorAsync(Guid empresaId, Guid setorId);
        Task AddFuncionarioAsync(Guid empresaId, Guid setorId, Guid funcionarioId, decimal salario);
        Task RemoveFuncionarioAsync(Guid empresaId, Guid setorId, Guid funcionarioId);

    }
}
