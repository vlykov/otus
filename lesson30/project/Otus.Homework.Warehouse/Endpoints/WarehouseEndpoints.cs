using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Otus.Homework.Warehouse.Infrastructure.Persistence;
using Otus.Homework.Common.ExceptionHandlers.Models;

namespace Otus.Homework.Warehouse.Endpoints;

public static class WarehouseEndpoints
{
    internal record ReservationDto(int Id, int OrderId, string Product, string Status, DateTimeOffset CreatedAt);

    internal static IEndpointRouteBuilder MapApplicationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var usersGroup = endpoints.MapGroup("/reservations");

        usersGroup.MapGet("", GetReservationsAsync);

        return endpoints;
    }

    internal static async Task<Results<Ok<List<ReservationDto>>, NotFound<ErrorResponse>>> GetReservationsAsync
    (
        WarehouseContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var notifications = await dbContext.ProductReservations
            .AsNoTracking()
            .OrderByDescending(_ => _.CreatedAt)
            .Select(reservation => new ReservationDto(reservation.Id, reservation.OrderId, reservation.Product, reservation.Status, reservation.CreatedAt))
            .ToListAsync(cancellationToken);

        return notifications.Any()
            ? TypedResults.Ok(notifications)
            : TypedResults.NotFound(new ErrorResponse("Бронирования товаров не существуют"));
    }
}
