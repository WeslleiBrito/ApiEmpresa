using ApiEmpresas.DTOs.Funcionario;
using ApiEmpresas.DTOs.Endereco;
using ApiEmpresas.DTOs.Setor;
using ApiEmpresas.Models;


namespace ApiEmpresas.DTOs.Empresa
{
    public class EmpresaResponseDTO
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? Cnpj { get; set; }
        public string? Telefone { get; set; }
        public TipoPessoa TipoPessoa { get; set; }
        public string? RegimeTributario { get; set; }
        public EnderecoDTO? Endereco { get; set; }
        public List<SetorDTO> Setores { get; set; } = [];
    }
}
