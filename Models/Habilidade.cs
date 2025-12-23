public class Habilidade
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public ICollection<FuncionarioHabilidade> Funcionarios { get; set; } = [];
}
