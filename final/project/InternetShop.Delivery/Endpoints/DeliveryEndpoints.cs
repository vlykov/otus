using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using InternetShop.Delivery.Infrastructure.Persistence;
using InternetShop.Common.ExceptionHandlers.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using InternetShop.Common.Authentication.Models;
using MassTransit;
using static InternetShop.Common.Contracts.MessageBroker.Events.Orders;
using static InternetShop.Common.Contracts.MessageBroker.Events.Delivery;

namespace InternetShop.Delivery.Endpoints;

public static class DeliveryEndpoints
{
    internal record DeliveryDto(int Id, int OrderId, string Status, DateTimeOffset CreatedAt);
    internal record SetDeliveriedDto(int OrderId);

    internal static IEndpointRouteBuilder MapApplicationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var usersGroup = endpoints.MapGroup("/")
            .RequireAuthorization();

        usersGroup.MapGet("", GetDeliveriesAsync);
        usersGroup.MapPost("/set-deliveried", SetDeliveriedAsync);

        return endpoints;
    }

    internal static async Task<Results<Ok<List<DeliveryDto>>, NotFound<ErrorResponse>>> GetDeliveriesAsync
    (
        CoreDbContext dbContext,
        ClaimsPrincipal principal,
        CancellationToken cancellationToken
    )
    {
        //var user = new User((ClaimsIdentity)principal.Identity!);

        var notifications = await dbContext.Deliveries
            .AsNoTracking()
            .OrderByDescending(_ => _.CreatedAt)
            .Select(reservation => new DeliveryDto(reservation.Id, reservation.OrderId, reservation.Status, reservation.CreatedAt))
            .ToListAsync(cancellationToken);

        return notifications.Count == 0
            ? TypedResults.NotFound(new ErrorResponse("Бронирования доставок не существуют"))
            : TypedResults.Ok(notifications);
    }

    internal static async Task<Results<Ok, BadRequest<ErrorResponse>, NotFound<ErrorResponse>>> SetDeliveriedAsync
    (
        [FromBody] SetDeliveriedDto request,
        CoreDbContext dbContext,
        IBus bus,
        ClaimsPrincipal principal,
        CancellationToken cancellationToken
    )
    {
        var user = new User((ClaimsIdentity)principal.Identity!);

        if (await dbContext.Deliveries.FirstOrDefaultAsync(reservation => reservation.OrderId == request.OrderId, cancellationToken) is not { } delivery)
        {
            return TypedResults.NotFound(new ErrorResponse($"Бронирование доставки для заказа с id {request.OrderId} не существует"));
        }
        delivery.Deliveried();

        await dbContext.SaveChangesAsync(cancellationToken);

        await bus.Publish(new DeliveryCompleted(delivery.OrderId), cancellationToken);

        return TypedResults.Ok();
    }
}
