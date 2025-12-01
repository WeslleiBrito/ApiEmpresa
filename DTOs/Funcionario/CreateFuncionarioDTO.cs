using ApiEmpresas.DTOs.Endereco;

namespace ApiEmpresas.DTOs.Funcionario
{
    public class CreateFuncionarioDTO
    {
        public string? Nome { get; set; }
        public decimal Salario { get; set; }
        public Guid EmpresaId { get; set; }
        public Guid ProfissaoId { get; set; }
        public CreateEnderecoDTO? Endereco { get; set; }
    }
}
