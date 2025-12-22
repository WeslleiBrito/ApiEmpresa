using ApiEmpresas.DTOs.Empresa;
using FluentValidation;
using ApiEmpresas.Helpers;

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
                .Must(CnpjValidatorHelper.IsCnpj).WithMessage("O CNPJ informado é inválido.");

            RuleFor(x => x.RegimeTributario)
                .IsInEnum().WithMessage("Regime tributário inválido.");

            RuleFor(x => x.Endereco)
                .NotNull().WithMessage("O endereço é obrigatório.")
                .SetValidator(new CreateEnderecoDTOValidator());
            
            RuleFor(x => x.Telefone)
                .MaximumLength(15).WithMessage("O telefone deve ter no máximo 15 caracteres.")
                // Regex para formato: (XX) 9XXXX-XXXX ou XX9XXXXXXXX
                .Matches(@"^\(?[1-9]{2}\)? ?(?:[2-8]|9[0-9])[0-9]{3}\-?[0-9]{4}$")
                .When(x => !string.IsNullOrEmpty(x.Telefone)) // Só valida o formato se o Telefone for preenchido
                .WithMessage("O telefone deve ser válido e incluir o DDD (Ex: (XX) 9XXXX-XXXX).");
        }
    }
}
