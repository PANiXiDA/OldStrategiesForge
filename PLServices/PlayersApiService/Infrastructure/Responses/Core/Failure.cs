namespace PlayersApiService.Infrastructure.Responses.Core;

public class Failure
{
    public Dictionary<string, string> Errors { get; } = new Dictionary<string, string>();

    public Failure() { }

    private Failure(string? error, string property = "")
    {
        Errors = new Dictionary<string, string>();

        if (!string.IsNullOrEmpty(error))
        {
            Errors[property] = error;
        }
    }

    public static Failure Create(string? error, string property = "")
    {
        return new Failure(error, property);
    }

    public Failure AddError(string error, string property = "")
    {
        Errors[property] = error;
        return this;
    }
}

