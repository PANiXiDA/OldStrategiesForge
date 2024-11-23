using Tools.RabbitMQ;

namespace ProfileService.Extensions.Helpers;

public static class RabbitMqHelper
{
    public static async Task<TResponse?> CallSafely<TRequest, TResponse>(
        IRabbitMQClient rabbitMqClient,
        TRequest request,
        string queue,
        TimeSpan timeout,
        ILogger logger,
        string timeoutMessage,
        string generalErrorMessage)
    {
        try
        {
            return await rabbitMqClient.CallAsync<TRequest, TResponse>(request, queue, timeout);
        }
        catch (TimeoutException ex)
        {
            logger.LogError(ex, "RabbitMQ timeout");
            throw RpcExceptionHelper.DeadlineExceeded(timeoutMessage);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "RabbitMQ error");
            throw RpcExceptionHelper.InternalError(generalErrorMessage);
        }
    }
}

