
using ApiEmpresas.DTOs.Setor;

namespace ApiEmpresas.DTOs.Empresa
{
    public class FuncionarioSetorResponseDTO
    {
        public Guid EmpresaId { get; set; }
        public string NomeEmpresa { get; set; } = string.Empty;

        public Guid SetorId { get; set; }
        public string NomeSetor { get; set; } = string.Empty;

        public decimal Salario { get; set; }
    }

}