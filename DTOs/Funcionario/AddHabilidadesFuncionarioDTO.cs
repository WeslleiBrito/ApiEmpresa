using System.ComponentModel.DataAnnotations;

namespace ApiEmpresas.DTOs.Funcionario
{
    public class AddHabilidadesFuncionarioDTO
    {
        [Required]
        [MinLength(1)]
        public List<Guid> HabilidadesIds { get; set; } = [];
    }
}
