using System.ComponentModel.DataAnnotations;

namespace ApiEmpresas.DTOs.Funcionario
{
    public class RemoveHabilidadesFuncionarioDTO
    {
        [Required]
        [MinLength(1)]
        public List<Guid> HabilidadesIds { get; set; } = [];
    }
}
