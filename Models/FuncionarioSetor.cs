using ApiEmpresas.Models;

public class FuncionarioSetor
{
    public Guid FuncionarioId { get; set; }
    public Funcionario? Funcionario { get; set; }

    public Guid SetorId { get; set; }
    public Setor? Setor { get; set; }
}
