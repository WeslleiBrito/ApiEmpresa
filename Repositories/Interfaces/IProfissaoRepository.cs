using ApiEmpresas.Models;

namespace ApiEmpresas.Repositories.Interfaces
{
    public interface IProfissaoRepository : IRepository<Profissao>
    {
        Task<bool> ExistsByNomeAsync(string nome);
    }
}
