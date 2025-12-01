using System.ComponentModel.DataAnnotations;

namespace ApiEmpresas.Models
{
    public class Endereco
    {
        [Required]
        [MaxLength(200)]
        public string? Logradouro { get; set; }

        [Required]
        [MaxLength(10)]
        public string? Numero { get; set; }

        [MaxLength(100)]
        public string? Complemento { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Bairro { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Cidade { get; set; }

        [Required]
        [MaxLength(2)]
        public string? Estado { get; set; }

        [Required]
        [MaxLength(11)]
        public string? Cep { get; set; }
    }
}
