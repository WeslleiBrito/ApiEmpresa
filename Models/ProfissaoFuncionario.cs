using ApiEmpresas.Models;


public class ProfissaoFuncionario
{
    public Guid ProfissaoId { get; set; }
    public Profissao? Profissao { get; set; }

    public int FuncionarioId { get; set; }
    public Funcionario? Funcionario { get; set; }
}
