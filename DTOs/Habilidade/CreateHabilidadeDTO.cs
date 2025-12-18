using System.ComponentModel.DataAnnotations;

namespace ApiEmpresas.DTOs.Habilidade
{
    public class CreateHabilidadeDTO
    {
        [Required]
        [MaxLength(120)]
        public string Nome { get; set; } = string.Empty;
    }
}
