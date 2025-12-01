using ApiEmpresas.DTOs.Funcionario;
using FluentValidation;

namespace ApiEmpresas.Validators
{
    public class CreateFuncionarioDTOValidator : AbstractValidator<CreateFuncionarioDTO>
    {
        public CreateFuncionarioDTOValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.");

            RuleFor(x => x.Salario)
                .GreaterThan(0).WithMessage("O salário deve ser maior que zero.");

            RuleFor(x => x.ProfissaoId)
                .NotEmpty().WithMessage("ProfissaoId é obrigatório.");

            RuleFor(x => x.EmpresaId)
                .NotEmpty().WithMessage("EmpresaId é obrigatório.");

            RuleFor(x => x.Endereco)
                .NotNull().WithMessage("Endereço é obrigatório.");
        }
    }
}
