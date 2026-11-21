using Coffee.Journal.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Coffee.Journal.API.ExceptionHandlers;

internal sealed class RequestValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not RequestValidationException validationException)
        {
            return false;
        }

        IResult result = Results.ValidationProblem(validationException.Errors);

        await result.ExecuteAsync(httpContext);

        return true;
    }
}
