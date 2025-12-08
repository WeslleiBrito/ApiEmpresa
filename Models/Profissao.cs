using System.ComponentModel.DataAnnotations;

namespace ApiEmpresas.Models
{
    public class Profissao
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(120)]
        public required string Nome { get; set; }

        public ICollection<Funcionario> Funcionarios { get; set; } = [];
    }
}
