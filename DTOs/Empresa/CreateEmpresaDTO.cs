using ApiEmpresas.DTOs.Endereco;
using ApiEmpresas.Models;

namespace ApiEmpresas.DTOs.Empresa
{
    public class CreateEmpresaDTO
    {
        public string? Nome { get; set; }
        public string? Cnpj { get; set; }
        public RegimeTributario RegimeTributario { get; set; }

        public CreateEnderecoDTO? Endereco { get; set; }
        public List<Guid> SetoresIds { get; set; } = [];
    }
}
