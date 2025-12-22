using ApiEmpresas.DTOs.Endereco;
using ApiEmpresas.Models;

namespace ApiEmpresas.DTOs.Empresa
{
    public class CreateEmpresaDTO
    {
        public required string Nome { get; set; }
        public required string Cnpj { get; set; }
        public required RegimeTributario RegimeTributario { get; set; }
        public required CreateEnderecoDTO Endereco { get; set; }
        public string? Telefone { get; set; }
    }
}
