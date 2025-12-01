using ApiEmpresas.DTOs.Endereco;
using ApiEmpresas.DTOs.Profissao;

namespace ApiEmpresas.DTOs.Funcionario
{
    public class FuncionarioResponseDTO
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public decimal Salario { get; set; }
        public ProfissaoDTO? Profissao { get; set; }
        public EnderecoDTO? Endereco { get; set; }
    }
}
