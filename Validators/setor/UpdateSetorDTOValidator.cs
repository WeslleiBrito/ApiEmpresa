using FluentValidation;

public class UpdateSetorDTOValidator : AbstractValidator<UpdateSetorDTO>
{
    public UpdateSetorDTOValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do setor é obrigatório.")
            .MaximumLength(30).WithMessage("O nome do setor deve ter no máximo 30 caracteres.");
    }
}
