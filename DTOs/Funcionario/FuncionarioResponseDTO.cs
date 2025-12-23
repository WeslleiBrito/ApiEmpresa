using ApiEmpresas.DTOs.Empresa;
using ApiEmpresas.DTOs.Endereco;
using ApiEmpresas.DTOs.Habilidade;
using ApiEmpresas.Models;

namespace ApiEmpresas.DTOs.Funcionario
{
    public class FuncionarioResponseDTO
{
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public string? Cpf { get; set; }
    public TipoPessoa TipoPessoa { get; set; }

    public List<FuncionarioSetorResponseDTO> Setores { get; set; } = [];

    public List<HabilidadeResponseDTO>? Habilidades { get; set; }

    public string? Telefone { get; set; }
    public EnderecoDTO? Endereco { get; set; }
}

}
