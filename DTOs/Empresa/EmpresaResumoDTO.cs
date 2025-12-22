
namespace ApiEmpresas.DTOs.Empresa
{
    public class EmpresaResumoDTO
    {
        public Guid Id { get; set; }
        public required string Nome { get; set; }
        public required string Cnpj { get; set; }
    }
}