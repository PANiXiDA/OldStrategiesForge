using Grpc.Core.Interceptors;
using Grpc.Core;
using Common;

namespace ProfileService.Extensions;

public class ExceptionInterceptor : Interceptor
{
    private readonly ILogger<ExceptionInterceptor> _logger;

    public ExceptionInterceptor(ILogger<ExceptionInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (RpcException ex)
        {
            _logger.LogError(ex, "gRPC вызов завершился с ошибкой: {Message}", ex.Status.Detail);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Необработанное исключение.");
            throw new RpcException(new Status(StatusCode.Internal, Constants.ErrorMessages.Unavailable));
        }
    }
}

