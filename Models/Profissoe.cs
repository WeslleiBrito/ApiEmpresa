using System.ComponentModel.DataAnnotations;

namespace ApiEmpresas.Models
{
    public class Profissao
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(120)]
        public string Nome { get; set; } = null!;

        [Required]
        public Guid SetorId { get; set; }

        public Setor? Setor { get; set; }

        public ICollection<Funcionario>? Funcionarios { get; set; } = [];
    }
}
