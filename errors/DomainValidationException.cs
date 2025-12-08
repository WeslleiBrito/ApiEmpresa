
using FluentValidation.Results;

namespace ApiEmpresas.Exceptions
{
    public class DomainValidationException : AppException
    {
        // Construtor para erros simples
        public DomainValidationException(string message) : base(message, 400)
        {
        }

        // Construtor para lista de erros (Dictionary)
        public DomainValidationException(Dictionary<string, string[]> errors) 
            : base("Erros de validação encontrados.", 400, errors)
        {
        }
    }
}