using System.ComponentModel.DataAnnotations;

namespace ApiEmpresas.Models
{
    public abstract class Pessoa
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(120)]
        public string? Nome { get; set; }

        [Required]
        public TipoPessoa TipoPessoa { get; set; }

        [Required]
        public Endereco Endereco { get; set; } = null!;

        [MaxLength(15)]
        public string? Telefone { get; set; }
    }
}
