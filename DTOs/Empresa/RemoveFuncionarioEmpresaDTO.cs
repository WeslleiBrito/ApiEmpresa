
using System.ComponentModel.DataAnnotations;

namespace ApiEmpresas.DTOs.Empresa
{
    public class RemoveFuncionarioEmpresaDTO
    {
        [Required]
        [MinLength(1, ErrorMessage = "Deve ser fornecido ao menos um funcionário.")]
        public required List<ItemFuncionarioDTO> Funcionarios { get; set; }
    }
}