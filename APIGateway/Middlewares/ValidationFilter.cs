using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using APIGateway.Infrastructure.Core;

namespace APIGateway.Filters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var failure = new Failure();

            foreach (var keyValuePair in context.ModelState)
            {
                foreach (var error in keyValuePair.Value.Errors)
                {
                    failure.AddError(
                        $"Status(StatusCode=\"PermissionDenied\", Detail=\"{error.ErrorMessage}\")",
                        keyValuePair.Key
                    );
                }
            }

            var response = RestApiResponse<object>.Fail(failure);

            context.Result = new JsonResult(response)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}