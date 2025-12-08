using ApiEmpresas.DTOs.Endereco;

namespace ApiEmpresas.DTOs.Funcionario
{
    public class UpdateFuncionarioDTO
    {
        public required string Nome { get; set; }
        public required decimal Salario { get; set; }
        public required Guid ProfissaoId { get; set; }
        public required CreateEnderecoDTO Endereco { get; set; }
        public required List<Guid> SetoresId { get; set; }
        public string? Telefone { get; set; } = null;

    }
}
