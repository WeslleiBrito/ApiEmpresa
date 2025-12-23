
using System.ComponentModel.DataAnnotations;

namespace ApiEmpresas.DTOs.Empresa
{
    public class ItemFuncionarioDTO
    {
        [Required(ErrorMessage = "O Id do funcionário é obrigatório.")]
        public Guid FuncionarioId { get; set; }
        [Required(ErrorMessage = "O Id do setor é obrigatório.")]
        public Guid SetorId { get; set; } 
        
        [Required(ErrorMessage = "O salário do funcionário no setor é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O salário deve ser maior que zero.")]
        public decimal Salario { get; set; }
    }
}