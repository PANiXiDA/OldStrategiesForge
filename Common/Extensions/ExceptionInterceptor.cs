using Grpc.Core.Interceptors;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Common.Extensions;
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
        catch (RpcException rpcEx) when (
            rpcEx.StatusCode == StatusCode.AlreadyExists ||
            rpcEx.StatusCode == StatusCode.NotFound ||
            rpcEx.StatusCode == StatusCode.FailedPrecondition ||
            rpcEx.StatusCode == StatusCode.PermissionDenied)
        {
            _logger.LogInformation(
                "gRPC бизнес-ошибка ({StatusCode}): {Detail}",
                rpcEx.StatusCode,
                rpcEx.Status.Detail);
            throw;
        }
        catch (RpcException rpcEx)
        {
            _logger.LogError(
                rpcEx,
                "gRPC вызов завершился с ошибкой: {Detail}",
                rpcEx.Status.Detail);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Необработанное исключение.");
            throw new RpcException(new Status(StatusCode.Internal, Constants.ErrorMessages.Unavailable));
        }
    }
}