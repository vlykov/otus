using Otus.Msa.Profile.Api.Dto.Common;
using Otus.Msa.Profile.Api.Middlewares.Constants;

namespace Otus.Msa.Profile.Api.Middlewares;

internal sealed class AuthorizationContext {
    public int Id { get; set; }
    public string Login { get; set; } = default!;
    public string? Email { get; set; }
}        

/// <summary>
///		Middleware проверки токена авторизации, предоставленным сервисом ЕАС4 в целях авторизации с приложением.
/// </summary>
internal sealed class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    
    public AuthorizationMiddleware(RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(next);

        _next = next;
    }

    public Task Invoke(HttpContext context, AuthorizationContext authorizationContext)
    {
        ArgumentNullException.ThrowIfNull(authorizationContext);

        context.Request.Headers.TryGetValue(CustomHeaders.AuthorizationUserId, out var headerUserId);
        context.Request.Headers.TryGetValue(CustomHeaders.AuthorizationUserLogin, out var headerUserLogin);
        context.Request.Headers.TryGetValue(CustomHeaders.AuthorizationUserEmail, out var headerUserEmail);

        var userId = headerUserId.FirstOrDefault();
        var userLogin = headerUserLogin.FirstOrDefault();
        var userEmail = headerUserEmail.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(userId) 
            || !int.TryParse(userId, out var id)
            || string.IsNullOrWhiteSpace(userLogin))
        {
            return WriteUnauthorizedErrorResponseAsync(context, "Значение идентификатора пользователя некорректное или незадано");
        }

        authorizationContext.Id = id;
        authorizationContext.Login = userLogin;
        authorizationContext.Email = userEmail;

        return _next(context);
    }

    private Task WriteUnauthorizedErrorResponseAsync(HttpContext context, string? details = null)
    {
        var message = "Unauthorized";

        var errorResponse = new ErrorResponse(message: message, details: details);
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return context.Response.WriteAsJsonAsync(errorResponse);
    }
}

public static class AuthorizationMiddlewareExtensions
{
    /// <summary>
    ///		Добавляет <see cref="AuthorizationMiddleware"/> к pipeline для проверки авторизации пользователя.
    /// </summary>
    public static IApplicationBuilder UseCustomAuthorization(
        this IApplicationBuilder app)
    {
        return app.UseMiddleware<AuthorizationMiddleware>();
    }
}
