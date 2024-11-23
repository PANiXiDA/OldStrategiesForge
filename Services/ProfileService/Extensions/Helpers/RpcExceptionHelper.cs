using Grpc.Core;

namespace ProfileService.Extensions.Helpers;

public static class RpcExceptionHelper
{
    public static RpcException Create(StatusCode statusCode, string message)
    {
        return new RpcException(new Status(statusCode, message));
    }

    public static RpcException AlreadyExists(string message) => Create(StatusCode.AlreadyExists, message);
    public static RpcException NotFound(string message) => Create(StatusCode.NotFound, message);
    public static RpcException InternalError(string message) => Create(StatusCode.Internal, message);
    public static RpcException DeadlineExceeded(string message) => Create(StatusCode.DeadlineExceeded, message);
    public static RpcException PermissionDenied(string message) => Create(StatusCode.PermissionDenied, message);
    public static RpcException FailedPrecondition(string message) => Create(StatusCode.FailedPrecondition, message);
}

