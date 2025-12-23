using ApiEmpresas.DTOs.Funcionario;

namespace ApiEmpresas.DTOs.Setor
{
    public class SetorDTO
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public List<FuncionarioResponseResumidoDTO> Funcionarios { get; set; } = [];
    }
}
