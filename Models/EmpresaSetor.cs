namespace ApiEmpresas.Models
{
    public class EmpresaSetor
    {
        public Guid EmpresaId { get; set; }
        public Empresa Empresa { get; set; } = null!;

        public Guid SetorId { get; set; }
        public Setor Setor { get; set; } = null!;

        public List<FuncionarioSetor> FuncionarioSetores { get; set; } = [];
    }
}
