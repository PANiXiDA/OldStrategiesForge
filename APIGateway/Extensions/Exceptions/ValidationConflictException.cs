namespace APIGateway.Extensions.Exceptions;

public class ValidationConflictException : Exception
{
    public string Property { get; }

    public ValidationConflictException(string message, string property) : base(message)
    {
        Property = property;
    }
}