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

        public override TipoPessoa TipoPessoa { get; set; } = TipoPessoa.Juridica;

        // Funcionários desta empresa
        public ICollection<Funcionario> Funcionarios { get; set; } = [];

        // N:N com Setor
        public ICollection<EmpresaSetor> Setores { get; set; } = [];
    }
}
