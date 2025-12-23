using System.ComponentModel.DataAnnotations;

public class RemoveSetorDTO
{
    [Required]
    [MinLength(1, ErrorMessage = "Deve ser fornecido ao menos um setor.")]
    public List<Guid> SetoresIds { get; set; } = [];
}