using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Otus.Homework.Orders.Infrastructure.Persistence;
using Otus.Homework.Common.ExceptionHandlers.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Otus.Homework.Orders.Domain;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Orders;

namespace Otus.Homework.Orders.Endpoints;

public static class OrderEndpoints
{
    internal record OrderDto(int Id, string Product, decimal TotalPrice, string Status, DateTimeOffset CreatedAt);
    internal record CreateOrderDto(string Product, decimal TotalPrice);

    internal static IEndpointRouteBuilder MapApplicationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var usersGroup = endpoints.MapGroup("/");

        usersGroup.MapPost("create", CreateOrderAsync);

        usersGroup.MapGet("", GetOrdersAsync);

        return endpoints;
    }

    internal static async Task<Ok<OrderDto>> CreateOrderAsync
        (
            [FromBody] CreateOrderDto createOrderDto,
            OrderContext dbContext,
            IBus bus,
            CancellationToken cancellationToken
        )
    {
        var order = new Order(createOrderDto.Product, createOrderDto.TotalPrice);

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        await bus.Publish(new OrderCreated(order.Id, order.Product, order.TotalPrice), cancellationToken);

        var orderResult = new OrderDto(order.Id, order.Product, order.TotalPrice, order.Status, order.CreatedAt);

        return TypedResults.Ok(orderResult);
    }

    internal static async Task<Results<Ok<List<OrderDto>>, NotFound<ErrorResponse>>> GetOrdersAsync
    (
        OrderContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var orders = await dbContext.Orders
            .AsNoTracking()
            .OrderByDescending(_ => _.CreatedAt)
            .Select(order => new OrderDto(order.Id, order.Product, order.TotalPrice, order.Status, order.CreatedAt))
            .ToListAsync(cancellationToken);

        return orders.Any()
            ? TypedResults.Ok(orders)
            : TypedResults.NotFound(new ErrorResponse("Заказы не существуют"));
    }
}
