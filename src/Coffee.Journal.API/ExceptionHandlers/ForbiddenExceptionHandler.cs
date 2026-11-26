using Coffee.Journal.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Coffee.Journal.API.ExceptionHandlers;

internal sealed class ForbiddenExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ForbiddenException)
        {
            return false;
        }

        IResult result = Results.Problem(statusCode: StatusCodes.Status403Forbidden, detail: exception.Message);

        await result.ExecuteAsync(httpContext);

        return true;
    }
}
