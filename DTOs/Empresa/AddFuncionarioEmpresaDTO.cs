using System.ComponentModel.DataAnnotations;

namespace ApiEmpresas.DTOs.Empresa
{
    public class AddFuncionarioEmpresaDTO
    {
        [Required]
        [MinLength(1, ErrorMessage = "Deve ser fornecido ao menos um funcionário.")]
        public List<ItemFuncionarioDTO> Funcionarios { get; set; } = null!;
    }
}