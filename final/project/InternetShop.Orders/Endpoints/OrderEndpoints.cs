using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using InternetShop.Orders.Infrastructure.Persistence;
using InternetShop.Common.ExceptionHandlers.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using InternetShop.Orders.Domain;
using static InternetShop.Common.Contracts.MessageBroker.Events.Orders;
using System.Security.Claims;
using InternetShop.Common.Authentication.Models;
using InternetShop.Common.Idempotency;

namespace InternetShop.Orders.Endpoints;

public static class OrderEndpoints
{
    internal record OrderDto(int Id, string Product, int Quantity, decimal TotalPrice, string Status, string? Reason, DateTimeOffset CreatedAt);
    internal record CreateOrderDto(string Product, int Quantity, decimal TotalPrice);

    internal static IEndpointRouteBuilder MapApplicationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var usersGroup = endpoints.MapGroup("/")
            .RequireAuthorization();

        usersGroup.MapPost("create", CreateOrderAsync);

        usersGroup.MapGet("", GetOrdersAsync);

        return endpoints;
    }

    internal static async Task<Results<Ok<OrderDto>, Conflict<ErrorResponse>, BadRequest<ErrorResponse>>> CreateOrderAsync
    (
        [FromHeader(Name = "X-Request-Id")] Guid RequestId,
        [FromBody] CreateOrderDto createOrderDto,
        CoreDbContext dbContext,
        IBus bus,
        IRequestManager requestManager,
        ClaimsPrincipal principal,
        CancellationToken cancellationToken
    )
    {
        if (createOrderDto.Quantity < 1)
        {
            return TypedResults.BadRequest(new ErrorResponse($"Неправильное количество товара '{createOrderDto.Quantity}'"));
        }

        if (await requestManager.ExistAsync(RequestId))
        {
            return TypedResults.Conflict(new ErrorResponse($"Запрос с id '{RequestId}' уже существует", "IdempotentError"));
        }

        var user = new User((ClaimsIdentity)principal.Identity!);

        var order = new Order(user.Id, createOrderDto.Product, createOrderDto.Quantity, createOrderDto.TotalPrice);
        var request = await requestManager.CreateRequestAsync<Order>(RequestId, order.Id.ToString());

        dbContext.Orders.Add(order);
        dbContext.ClientRequests.Add(request);
        await dbContext.SaveChangesAsync(cancellationToken);

        await bus.Publish(new OrderCreated(order.Id, user.Id, order.Product, order.Quantity, order.TotalPrice), cancellationToken);

        var orderResult = new OrderDto(order.Id, order.Product, order.Quantity, order.TotalPrice, order.Status, order.Reason, order.CreatedAt);

        return TypedResults.Ok(orderResult);
    }

    internal static async Task<Results<Ok<List<OrderDto>>, NotFound<ErrorResponse>>> GetOrdersAsync
    (
        CoreDbContext dbContext,
        ClaimsPrincipal principal,
        CancellationToken cancellationToken
    )
    {
        //var user = new User((ClaimsIdentity)principal.Identity!);

        var orders = await dbContext.Orders
            .AsNoTracking()
            .OrderByDescending(_ => _.CreatedAt)
            .Select(order => new OrderDto(order.Id, order.Product, order.Quantity, order.TotalPrice, order.Status, order.Reason, order.CreatedAt))
            .ToListAsync(cancellationToken);

        return orders.Count == 0
            ? TypedResults.NotFound(new ErrorResponse("Заказы не существуют"))
            : TypedResults.Ok(orders);
    }
}
