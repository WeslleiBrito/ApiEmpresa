using ApiEmpresas.DTOs.Endereco;
using FluentValidation;

namespace ApiEmpresas.Validators
{
    public class CreateEnderecoDTOValidator : AbstractValidator<CreateEnderecoDTO>
    {
        public CreateEnderecoDTOValidator()
        {
            RuleFor(x => x.Logradouro)
                .NotEmpty().WithMessage("O logradouro é obrigatório.")
                .MaximumLength(200);

            RuleFor(x => x.Numero)
                .NotEmpty().WithMessage("O número é obrigatório.");

            RuleFor(x => x.Bairro)
                .NotEmpty().WithMessage("O bairro é obrigatório.");

            RuleFor(x => x.Cidade)
                .NotEmpty().WithMessage("A cidade é obrigatória.");

            // AQUI ESTÁ A REGRA DOS 2 CARACTERES
            RuleFor(x => x.Estado)
                .NotEmpty().WithMessage("O estado é obrigatório.")
                .Length(2).WithMessage("O estado deve ser a sigla (UF) de 2 letras (Ex: BA, SP).");

            RuleFor(x => x.Cep)
                .NotEmpty().WithMessage("O CEP é obrigatório.")
                // Opcional: Regex de CEP
                .Matches(@"^\d{8}$|^\d{5}-\d{3}$").WithMessage("O CEP deve estar no formato 00000000 ou 00000-000.");
        }
    }
}