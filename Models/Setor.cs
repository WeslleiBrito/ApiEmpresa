using System.ComponentModel.DataAnnotations;

namespace ApiEmpresas.Models
{
    public class Setor
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(120)]
        public string Nome { get; set; } = null!;

        // Setor global → pode ser usado por várias empresas
        public ICollection<EmpresaSetor>? Empresas { get; set; } = [];

        // Profissões pertencem a ESTE setor
        public ICollection<Profissao>? Profissoes { get; set; } = [];
    }
}
