using ApiEmpresas.Models;

public class PatchEmpresaDTO
{
    public string? Nome { get; set; }
    public string? Telefone { get; set; }
    public RegimeTributario? RegimeTributario { get; set; }

    public PatchEnderecoDTO? Endereco { get; set; }
}