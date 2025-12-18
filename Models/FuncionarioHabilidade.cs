using ApiEmpresas.Models;
public class FuncionarioHabilidade
{
    public Guid FuncionarioId { get; set; }
    public Funcionario? Funcionario { get; set; }

    public Guid HabilidadeId { get; set; }
    public Habilidade? Habilidade { get; set; }

    public DateTime DataObtencao { get; set; } = DateTime.UtcNow;
    public string? Observacao { get; set; }
}
