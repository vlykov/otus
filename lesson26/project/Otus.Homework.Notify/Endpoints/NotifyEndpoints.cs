using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Otus.Homework.Notify.Infrastructure.Persistence;
using Otus.Homework.Common.Authentication.Models;
using Otus.Homework.Common.ExceptionHandlers.Models;

namespace Otus.Homework.Notify.Endpoints;

public static class NotifyEndpoints
{
    internal record NotificationDto(int UserId, string Message, DateTimeOffset CreatedAt);

    internal static IEndpointRouteBuilder MapApplicationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var usersGroup = endpoints.MapGroup("/emails");

        usersGroup.MapGet("", GetNotifyMessagesAsync)
            .RequireAuthorization();

        return endpoints;
    }

    internal static async Task<Results<Ok<List<NotificationDto>>, NotFound<ErrorResponse>>> GetNotifyMessagesAsync
    (
        ClaimsPrincipal principal,
        NotifyContext dbContext,
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

        return notifications.Any()
            ? TypedResults.Ok(notifications)
            : TypedResults.NotFound(new ErrorResponse("Оповещения пользователя не существуют"));
    }
}
