namespace ApiEmpresas.DTOs.Habilidade
{
    public class HabilidadeResponseDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao {get; set;} = string.Empty;
    }
}
