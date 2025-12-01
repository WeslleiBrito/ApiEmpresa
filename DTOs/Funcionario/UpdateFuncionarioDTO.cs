using ApiEmpresas.DTOs.Endereco;

namespace ApiEmpresas.DTOs.Funcionario
{
    public class UpdateFuncionarioDTO
    {
        public string? Nome { get; set; }
        public decimal Salario { get; set; }

        // Permite alterar a profissão do funcionário
        public Guid ProfissaoId { get; set; }

        // Opcional: permite alterar endereço
        public CreateEnderecoDTO? Endereco { get; set; }
    }
}
