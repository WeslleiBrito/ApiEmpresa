using ApiEmpresas.DTOs.Habilidade;
using FluentValidation;

namespace ApiEmpresas.Validators
{
    public class CreateHabilidadeDTOValidator : AbstractValidator<CreateHabilidadeDTO>
    {
        public CreateHabilidadeDTOValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome da habilidade é obrigatório.")
                .MaximumLength(100).WithMessage("O nome da habilidade deve ter no máximo 100 caracteres.");

            RuleFor(x => x.Descricao)
                .MaximumLength(500).WithMessage("A descrição da habilidade deve ter no máximo 500 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Descricao));
        }
    }
}