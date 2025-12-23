
namespace ApiEmpresas.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string resourceName) 
            : base($"{resourceName} não encontrado(a).", 404)
        {
        }
        
        // Sobrecarga para mensagem customizada
         public NotFoundException(string message, bool isCustomMessage) 
            : base(message, 404)
        {
        }

        public NotFoundException(string message, IEnumerable<object> notFoundIds)
            : base(
                message: message, 
                statusCode: 404, 
                errors: notFoundIds // <--- LISTA DE ITENS NÃO ENCONTRADOS
            )
        {
        }
    }
}