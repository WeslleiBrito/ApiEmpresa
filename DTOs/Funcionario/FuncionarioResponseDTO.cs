using ApiEmpresas.DTOs.Endereco;
using ApiEmpresas.Models;

namespace ApiEmpresas.DTOs.Funcionario
{
    public class FuncionarioResponseDTO
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public required TipoPessoa TipoPessoa { get; set; }
        public decimal Salario { get; set; }
        public required Dictionary<string, object> Profissao { get; set; }
        public Dictionary<string, object>? Empresa { get; set; }
        public List<Dictionary<string, object>>? Setores { get; set; }
        public string? Telefone { get; set; } = null;
        public EnderecoDTO? Endereco { get; set; }
    }
}
