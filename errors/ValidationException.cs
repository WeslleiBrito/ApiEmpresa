public class ValidationException : Exception
{
    public Dictionary<string, string[]> Errors { get; set; }

    public ValidationException(string field, string message)
    {
        Errors = new Dictionary<string, string[]>
        {
            { field, new[] { message } }
        };
    }

    public ValidationException(Dictionary<string, string[]> errors)
    {
        Errors = errors;
    }
}
