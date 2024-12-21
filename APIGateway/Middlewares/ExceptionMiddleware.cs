using APIGateway.Extensions.Exceptions;
using APIGateway.Infrastructure.Extensions;
using Common.Constants;
using Grpc.Core;
using System.Text.Json;

namespace APIGateway.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (JsonException jsonEx)
        {
            await HandleJsonExceptionAsync(context, jsonEx);
        }
        catch (ValidationConflictException conflictEx)
        {
            _logger.LogWarning(conflictEx, "Конфликт проверки для свойства {Property}", conflictEx.Property);
            await HandleConflictExceptionAsync(context, conflictEx);
        }
        catch (RpcException rpcEx)
        {
            _logger.LogError(rpcEx, "gRPC вызов завершился с ошибкой.");
            await HandleGrpcExceptionAsync(context, rpcEx);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Неправильная операция: {Message}", ex.Message);
            await HandleInvalidOperationExceptionAsync(context, ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Неавторизованный доступ: {Message}", ex.Message);
            await HandleUnauthorizedAccessExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Необработанное исключение.");
            await HandleGeneralExceptionAsync(context, ex);
        }
    }

    private Task HandleConflictExceptionAsync(HttpContext context, ValidationConflictException ex)
    {
        context.Response.StatusCode = StatusCodes.Status409Conflict;

        var failure = Failure.Create(ex.Message, ex.Property);
        var response = RestApiResponse<object>.Fail(failure);

        context.Response.ContentType = "application/json";

        return context.Response.WriteAsJsonAsync(response);
    }
    private Task HandleGrpcExceptionAsync(HttpContext context, RpcException ex)
    {
        context.Response.StatusCode = ex.StatusCode switch
        {
            Grpc.Core.StatusCode.NotFound => StatusCodes.Status404NotFound,
            Grpc.Core.StatusCode.PermissionDenied => StatusCodes.Status403Forbidden,
            Grpc.Core.StatusCode.AlreadyExists => StatusCodes.Status409Conflict,
            Grpc.Core.StatusCode.FailedPrecondition => StatusCodes.Status412PreconditionFailed,
            _ => StatusCodes.Status500InternalServerError
        };

        var failure = Failure.Create(ex.Message, ex.Status.StatusCode.ToString());
        var response = RestApiResponse<object>.Fail(failure);

        context.Response.ContentType = "application/json";

        return context.Response.WriteAsJsonAsync(response);
    }

    private Task HandleInvalidOperationExceptionAsync(HttpContext context, InvalidOperationException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var failure = Failure.Create(ex.Message, "InvalidOperation");
        var response = RestApiResponse<object>.Fail(failure);

        context.Response.ContentType = "application/json";

        return context.Response.WriteAsJsonAsync(response);
    }

    private Task HandleUnauthorizedAccessExceptionAsync(HttpContext context, UnauthorizedAccessException ex)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;

        var failure = Failure.Create(ex.Message, "UnauthorizedAccess");
        var response = RestApiResponse<object>.Fail(failure);

        context.Response.ContentType = "application/json";

        return context.Response.WriteAsJsonAsync(response);
    }

    private Task HandleGeneralExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var failure = Failure.Create(ErrorMessages.Unavailable, StatusCodes.Status500InternalServerError.ToString());

        var response = RestApiResponse<object>.Fail(failure);

        context.Response.ContentType = "application/json";

        return context.Response.WriteAsJsonAsync(response);
    }

    private Task HandleJsonExceptionAsync(HttpContext context, JsonException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var failure = new Failure().AddError("Invalid JSON format.", "request");
        var response = RestApiResponse<object>.Fail(failure);

        context.Response.ContentType = "application/json";

        return context.Response.WriteAsJsonAsync(response);
    }
}