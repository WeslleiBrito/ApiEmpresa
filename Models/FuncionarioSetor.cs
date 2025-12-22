using ApiEmpresas.Models;

public class    FuncionarioSetor
{

    public Guid EmpresaId { get; set; }
    public Empresa? Empresa { get; set; }
    public Guid FuncionarioId { get; set; }
    public Funcionario? Funcionario { get; set; }
    public Guid SetorId { get; set; }
    public Setor? Setor { get; set; }
    public decimal Salario { get; set; }
}
