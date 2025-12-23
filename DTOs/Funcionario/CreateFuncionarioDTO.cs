using ApiEmpresas.DTOs.Endereco;

namespace ApiEmpresas.DTOs.Funcionario
{
    public class CreateFuncionarioDTO
    {
        public required string Nome { get; set; }
        public required string Cpf { get; set; } 
        public required CreateEnderecoDTO Endereco { get; set; }
        public string? Telefone { get; set; } = null;
    }
}
