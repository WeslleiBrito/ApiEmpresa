// Errors/ErrorHandlingMiddleware.cs
using ApiEmpresas.Exceptions; // Importe o namespace das exceções
using System.Text.Json;

namespace ApiEmpresas.Errors
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger; 

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            // Padrão de resposta
            var response = new
            {
                status = 500,
                error = "Erro Interno",
                message = "Ocorreu um erro inesperado. Tente novamente mais tarde.",
                details = (object?)null // Detalhes apenas se necessário
            };

            switch (ex)
            {
                // CASO 1: Exceções controladas da nossa aplicação (404, 403, 400, 409)
                case AppException appEx:
                    context.Response.StatusCode = appEx.StatusCode;
                    response = new
                    {
                        status = appEx.StatusCode,
                        error = appEx.GetType().Name.Replace("Exception", ""), // Ex: NotFound
                        message = appEx.Message,
                        details = appEx.Errors // Aqui entram os erros de validação se houver, mas nessa versão vai ser validado no mapper
                    };
                    break;

                // CASO 2: Exceção Genérica (Bug não tratado - Erro 500)
                default:
                    context.Response.StatusCode = 500;
                    _logger.LogError(ex, "Erro não tratado ocorrido."); 
                    // Em produção, não mostre ex.Message, deixe a mensagem genérica.
                    // Em dev, você pode colocar ex.Message para debug.
                    break;
            }

            var jsonResult = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true // Opcional, facilita leitura
            });

            return context.Response.WriteAsync(jsonResult);
        }
    }
}