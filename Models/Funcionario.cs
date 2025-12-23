using System.ComponentModel.DataAnnotations;

namespace ApiEmpresas.Models
{
    public class Funcionario : Pessoa
    {
        [Required]
        public decimal Salario { get; set; }

        [Required]
        public string Cpf { get; set; } = null!;
        
        public override TipoPessoa TipoPessoa { get; set; } = TipoPessoa.Fisica;

        // N:N com Setor
        public ICollection<FuncionarioSetor> FuncionarioSetorVinculo { get; set; } = [];
        // N:N com Habilidade
        public ICollection<FuncionarioHabilidade> Habilidades { get; set; } = [];
    }
}
