using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using InternetShop.Warehouse.Infrastructure.Persistence;
using InternetShop.Common.ExceptionHandlers.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using InternetShop.Common.Authentication.Models;

namespace InternetShop.Warehouse.Endpoints;

public static class ReservationEndpoints
{
    internal record ReservationDto(int Id, int OrderId, int ProductId, int Quantity, string Status, DateTimeOffset CreatedAt);
    internal record HandoverDto(int OrderId);

    internal static IEndpointRouteBuilder MapReservationsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var usersGroup = endpoints.MapGroup("/reservations")
            .RequireAuthorization();

        usersGroup.MapGet("", GetReservationsAsync);
        usersGroup.MapPost("/handover", HandoverReservationAsync);

        return endpoints;
    }

    internal static async Task<Results<Ok<List<ReservationDto>>, NotFound<ErrorResponse>>> GetReservationsAsync
    (
        CoreDbContext dbContext,
        ClaimsPrincipal principal,
        CancellationToken cancellationToken
    )
    {
        //var user = new User((ClaimsIdentity)principal.Identity!);

        var notifications = await dbContext.ProductReservations
            .AsNoTracking()
            .OrderByDescending(_ => _.CreatedAt)
            .Select(reservation => new ReservationDto(reservation.Id, reservation.OrderId, reservation.ProductId, reservation.Quantity, reservation.Status, reservation.CreatedAt))
            .ToListAsync(cancellationToken);

        return notifications.Count == 0
            ? TypedResults.NotFound(new ErrorResponse("Бронирования товаров не существуют"))
            : TypedResults.Ok(notifications);
    }

    internal static async Task<Results<Ok, BadRequest<ErrorResponse>, NotFound<ErrorResponse>>> HandoverReservationAsync
    (
        [FromBody] HandoverDto request,
        CoreDbContext dbContext,
        ClaimsPrincipal principal,
        CancellationToken cancellationToken
    )
    {
        var user = new User((ClaimsIdentity)principal.Identity!);

        var reservations = await dbContext.ProductReservations.Where(reservation => reservation.OrderId == request.OrderId).ToListAsync(cancellationToken);

        if (reservations.Count == 0)
        {
            return TypedResults.NotFound(new ErrorResponse($"Резерв заказа с id {request.OrderId} не существует"));
        }

        foreach (var reservation in reservations)
        {
            reservation.HandoverToDelivery();
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok();
    }
}
