using ApiEmpresas.DTOs.Empresa;
using FluentValidation;

namespace ApiEmpresas.Validators
{
    public class CreateEmpresaDTOValidator : AbstractValidator<CreateEmpresaDTO>
    {
        public CreateEmpresaDTOValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(120);

            RuleFor(x => x.Cnpj)
                .NotEmpty().WithMessage("O CNPJ é obrigatório.")
                .Length(14, 18).WithMessage("O CNPJ deve ter entre 14 e 18 caracteres.");

            RuleFor(x => x.RegimeTributario)
                .IsInEnum().WithMessage("Regime tributário inválido.");

            RuleFor(x => x.Endereco)
                .NotNull().WithMessage("O endereço é obrigatório.");

            RuleForEach(x => x.SetoresIds)
                .NotEmpty().WithMessage("Cada setorId deve ser informado.");
        }
    }
}
