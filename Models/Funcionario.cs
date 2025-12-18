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
        // Empresa obrigatória
        [Required]
        public Guid EmpresaId { get; set; }
        [Required]
        public Empresa Empresa { get; set; } = null!;
        // Profissão obrigatória
        [Required]
        public Guid ProfissaoId { get; set; }
        
        public Profissao Profissao { get; set; } = null!;

        // N:N com Setor
        public ICollection<FuncionarioSetor> Setores { get; set; } = [];
        // N:N com Habilidade
        public ICollection<FuncionarioHabilidade> Habilidades { get; set; } = [];
    }
}
