
namespace ApiEmpresas.Exceptions
{
    public class ForbiddenException : AppException
    {

        public ForbiddenException(string message, int? statusCode = 403) 
            : base(message, statusCode ?? 403)
        {
        }
    }
}