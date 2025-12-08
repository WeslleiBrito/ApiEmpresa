using System.ComponentModel.DataAnnotations;

namespace ApiEmpresas.Models
{
    public abstract class Pessoa
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(120)]
        public required string Nome { get; set; }

        [Required]
        public abstract TipoPessoa TipoPessoa { get; set; }

        [Required]
        public required Endereco Endereco { get; set; }

        [MaxLength(15)]
        public string? Telefone { get; set; }

    }
}
