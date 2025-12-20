using System.Data;
using ApiEmpresas.Helpers;
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

            RuleFor(x => x.Cpf)
                .NotEmpty().WithMessage("O CPF é obrigatório.")
                .Must(CpfValidatorHelper.IsCpf).WithMessage("O CPF informado é inválido.");

            RuleFor(x => x.Salario)
                .GreaterThan(0).WithMessage("O salário deve ser maior que zero.");

            RuleFor(x => x.EmpresaId)
                .NotEmpty().WithMessage("EmpresaId é obrigatório.");

            RuleFor(x => x.SetoresId)
                .NotEmpty().WithMessage("SetorId é obrigatório.");
                
            RuleFor(x => x.Endereco)
                .NotNull().WithMessage("Endereço é obrigatório.")
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
