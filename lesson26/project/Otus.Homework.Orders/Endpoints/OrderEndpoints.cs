using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Otus.Homework.Orders.Infrastructure.Persistence;
using Otus.Homework.Common.Authentication.Models;
using Otus.Homework.Common.ExceptionHandlers.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Otus.Homework.Orders.Domain;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Orders;

namespace Otus.Homework.Orders.Endpoints;

public static class OrderEndpoints
{
    internal record OrderDto(Guid Id, int UserId, decimal TotalPrice, string Status, DateTimeOffset CreatedAt);
    internal record CreateOrderDto(decimal TotalPrice);

    internal static IEndpointRouteBuilder MapApplicationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var usersGroup = endpoints.MapGroup("/");

        usersGroup.MapPost("create", CreateOrderAsync)
            .RequireAuthorization();

        usersGroup.MapGet("", GetOrdersAsync)
            .RequireAuthorization();

        return endpoints;
    }

    internal static async Task<Ok<OrderDto>> CreateOrderAsync
        (
            [FromBody] CreateOrderDto createOrderDto,
            ClaimsPrincipal principal,
            OrderContext dbContext,
            IBus bus,
            CancellationToken cancellationToken
        )
    {
        var user = new User((ClaimsIdentity)principal.Identity!);

        var order = new Order(user.Id, createOrderDto.TotalPrice);

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        await bus.Publish(new OrderCreated(order.Id, order.UserId, order.TotalPrice), cancellationToken);

        var orderResult = new OrderDto(order.Id, order.UserId, order.TotalPrice, order.Status, order.CreatedAt);
        return TypedResults.Ok(orderResult);
    }

    internal static async Task<Results<Ok<List<OrderDto>>, NotFound<ErrorResponse>>> GetOrdersAsync
    (
        ClaimsPrincipal principal,
        OrderContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var user = new User((ClaimsIdentity)principal.Identity!);

        var orders = await dbContext.Orders
            .AsNoTracking()
            .Where(_ => _.UserId == user.Id)
            .OrderByDescending(_ => _.CreatedAt)
            .Select(notify => new OrderDto(notify.Id, notify.UserId, notify.TotalPrice, notify.Status, notify.CreatedAt))
            .ToListAsync(cancellationToken);

        return orders.Any()
            ? TypedResults.Ok(orders)
            : TypedResults.NotFound(new ErrorResponse("Заказы пользователя не существуют"));
    }
}
