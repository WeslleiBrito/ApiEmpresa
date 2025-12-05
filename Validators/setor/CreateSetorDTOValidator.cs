using ApiEmpresas.DTOs.Setor;
using FluentValidation;

namespace ApiEmpresas.Validators
{
    public class CreateSetorDTOValidator : AbstractValidator<CreateSetorDTO>
    {
        public CreateSetorDTOValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome do setor é obrigatório.")
                .MaximumLength(30).WithMessage("O nome do setor deve ter no máximo 30 caracteres.");
        }
    }
}
