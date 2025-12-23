using System.ComponentModel.DataAnnotations;

namespace ApiEmpresas.DTOs.Habilidade
{
    public class CreateHabilidadeDTO
    {
        [Required]
        [MaxLength(120)]
        [MinLength(3)]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Descricao { get; set; }
    }
}
