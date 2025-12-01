using ApiEmpresas.Data;
using ApiEmpresas.Models;
using ApiEmpresas.Repositories.Interfaces;

namespace ApiEmpresas.Repositories.Implementations
{
    public class SetorRepository : Repository<Setor>, ISetorRepository
    {
        public SetorRepository(AppDbContext context) : base(context)
        {
        }
    }
}
