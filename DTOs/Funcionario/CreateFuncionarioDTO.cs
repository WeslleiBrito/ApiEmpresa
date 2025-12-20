using ApiEmpresas.DTOs.Endereco;

namespace ApiEmpresas.DTOs.Funcionario
{
    public class CreateFuncionarioDTO
    {
        public required string Nome { get; set; }
        public required decimal Salario { get; set; }
        public required string Cpf { get; set; } 
        public required Guid EmpresaId { get; set; }
        public List<Guid> HabilidadesId { get; set; } = [];
        public required List<Guid> SetoresId { get; set; } = null!;
        public required CreateEnderecoDTO Endereco { get; set; }
        public string? Telefone { get; set; } = null;
    }
}
