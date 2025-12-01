namespace ApiEmpresas.DTOs.Profissao
{
    public class CreateProfissaoDTO
    {
        public string? Nome { get; set; }

        // Id do Setor ao qual a profissão pertence (obrigatório no payload)
        public Guid SetorId { get; set; }
    }
}
