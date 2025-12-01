using ApiEmpresas.DTOs.Funcionario;
using FluentValidation;

namespace ApiEmpresas.Validators
{
    public class UpdateFuncionarioDTOValidator : AbstractValidator<UpdateFuncionarioDTO>
    {
        public UpdateFuncionarioDTOValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.");

            RuleFor(x => x.Salario)
                .GreaterThan(0).WithMessage("O salário deve ser maior que zero.");

            RuleFor(x => x.ProfissaoId)
                .NotEmpty().WithMessage("ProfissaoId é obrigatório.");
        }
    }
}
