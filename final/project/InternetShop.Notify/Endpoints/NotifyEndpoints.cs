using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using InternetShop.Notify.Infrastructure.Persistence;
using InternetShop.Common.Authentication.Models;
using InternetShop.Common.ExceptionHandlers.Models;

namespace InternetShop.Notify.Endpoints;

public static class NotifyEndpoints
{
    internal record NotificationDto(int UserId, string Message, DateTimeOffset CreatedAt);

    internal static IEndpointRouteBuilder MapApplicationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var usersGroup = endpoints.MapGroup("/emails")
            .RequireAuthorization();

        usersGroup.MapGet("", GetNotifyMessagesAsync);

        return endpoints;
    }

    internal static async Task<Results<Ok<List<NotificationDto>>, NotFound<ErrorResponse>>> GetNotifyMessagesAsync
    (
        ClaimsPrincipal principal,
        CoreDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var user = new User((ClaimsIdentity)principal.Identity!);

        var notifications = await dbContext.Notifications
            .AsNoTracking()
            .Where(_ => _.UserId == user.Id)
            .OrderByDescending(_ => _.CreatedAt)
            .Select(notify => new NotificationDto(notify.UserId, notify.Message, notify.CreatedAt))
            .ToListAsync(cancellationToken);

        return notifications.Count == 0
            ? TypedResults.NotFound(new ErrorResponse("Оповещения пользователя не существуют"))
            : TypedResults.Ok(notifications);
    }
}
