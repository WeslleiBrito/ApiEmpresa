using ApiEmpresas.DTOs.Endereco;
using ApiEmpresas.DTOs.Setor;

namespace ApiEmpresas.DTOs.Empresa
{
    public class EmpresaResponseDTO
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? Cnpj { get; set; }
        public string? RegimeTributario { get; set; }
        public EnderecoDTO? Endereco { get; set; }
        public List<SetorDTO> Setores { get; set; } = [];
    }
}
