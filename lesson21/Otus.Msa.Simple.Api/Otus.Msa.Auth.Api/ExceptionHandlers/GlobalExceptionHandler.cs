using Microsoft.AspNetCore.Diagnostics;
using Otus.Msa.Auth.Api.Dto.Common;

namespace Otus.Msa.Auth.Api.ExceptionHandlers;

/// <summary>
///		Глобальный обработчик исключений для всех необработанных исключений, возникающих в приложении.
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    /// <inheritdoc/>
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ErrorResponse errorResponse = new ErrorResponse(message: "internal_error", details: $"{exception.GetType().Name}: '{exception.Message}'");

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken)
            .ConfigureAwait(false);

        return true;
    }
}
