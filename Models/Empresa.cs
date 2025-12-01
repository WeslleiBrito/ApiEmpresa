using System.ComponentModel.DataAnnotations;

namespace ApiEmpresas.Models
{
    public class Empresa : Pessoa
    {
        [Required]
        [MaxLength(18)]
        public string? Cnpj { get; set; }

        [Required]
        public RegimeTributario RegimeTributario { get; set; }

        // N:N com Setor
        public ICollection<EmpresaSetor> Setores { get; set; } = [];
    }
}
