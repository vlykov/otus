using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Otus.Homework.Delivery.Infrastructure.Persistence;
using Otus.Homework.Common.ExceptionHandlers.Models;

namespace Otus.Homework.Delivery.Endpoints;

public static class DeliveryEndpoints
{
    internal record DeliveryDto(int Id, int OrderId, string Product, string Status, DateTimeOffset CreatedAt);

    internal static IEndpointRouteBuilder MapApplicationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var usersGroup = endpoints.MapGroup("/");

        usersGroup.MapGet("", GetDeliveriesAsync);

        return endpoints;
    }

    internal static async Task<Results<Ok<List<DeliveryDto>>, NotFound<ErrorResponse>>> GetDeliveriesAsync
    (
        DeliveryContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var notifications = await dbContext.Couriers
            .AsNoTracking()
            .OrderByDescending(_ => _.CreatedAt)
            .Select(reservation => new DeliveryDto(reservation.Id, reservation.OrderId, reservation.Product, reservation.Status, reservation.CreatedAt))
            .ToListAsync(cancellationToken);

        return notifications.Any()
            ? TypedResults.Ok(notifications)
            : TypedResults.NotFound(new ErrorResponse("Бронирования доставок не существуют"));
    }
}
