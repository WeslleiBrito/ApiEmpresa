using System.ComponentModel.DataAnnotations;

namespace ApiEmpresas.Models
{
    public class Funcionario : Pessoa
    {
        [Required]
        public decimal Salario { get; set; }

        [Required]
        public Guid EmpresaId { get; set; }

        [Required]
        public Guid ProfissaoId { get; set; }

        public Profissao? Profissao { get; set; }
    }
}
