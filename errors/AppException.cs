using System.Net;

namespace ApiEmpresas.Exceptions
{
    public abstract class AppException : Exception
    {
        public int StatusCode { get; }
        public object? Errors { get; }

        protected AppException(string message, int statusCode, object? errors = null) 
            : base(message)
        {
            StatusCode = statusCode;
            Errors = errors;
        }
    }
}