namespace ApiEmpresas.DTOs.Profissao
{
    public class ProfissaoResponseDTO
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public Guid SetorId { get; set; }

        // Opcional: incluir nome do setor para conveniência do cliente
        public string? SetorNome { get; set; }
    }
}
