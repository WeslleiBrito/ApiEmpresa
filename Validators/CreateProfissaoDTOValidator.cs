using ApiEmpresas.DTOs.Profissao;
using FluentValidation;

namespace ApiEmpresas.Validators
{
    public class CreateProfissaoDTOValidator : AbstractValidator<CreateProfissaoDTO>
    {
        public CreateProfissaoDTOValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome da profissão é obrigatório.");

            RuleFor(x => x.SetorId)
                .NotEmpty().WithMessage("O SetorId é obrigatório.");
        }
    }
}
