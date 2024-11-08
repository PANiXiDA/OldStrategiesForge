namespace ProfileApiService.Infrastructure.Extensions;

public static class RestApiResponseBuilder<T>
{
    public static RestApiResponse<T> Success(T payload) => RestApiResponse<T>.Success(payload);

    public static RestApiResponse<T> Fail(
        string error,
        string property) => RestApiResponse<T>.Fail(Failure.Create(error, property));
}

