using ApiEmpresas.DTOs.Endereco;
using ApiEmpresas.Models;

namespace ApiEmpresas.DTOs.Empresa
{
    public class UpdateEmpresaDTO
    {
        public string Nome { get; set; } = default!;
        public string Cnpj { get; set; } = default!;
        public RegimeTributario RegimeTributario { get; set; }
        public CreateEnderecoDTO Endereco { get; set; } = default!;
        public List<Guid> SetoresIds { get; set; } = [];
    }
}
