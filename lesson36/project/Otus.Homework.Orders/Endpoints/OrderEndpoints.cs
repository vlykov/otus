using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Otus.Homework.Orders.Infrastructure.Persistence;
using Otus.Homework.Common.ExceptionHandlers.Models;
using Microsoft.AspNetCore.Mvc;
using Otus.Homework.Orders.Domain;
using Otus.Homework.Common.Idempotency;

namespace Otus.Homework.Orders.Endpoints;

public static class OrderEndpoints
{
    internal record OrderDto(Guid Id, string Product, decimal TotalPrice, DateTimeOffset CreatedAt);
    internal record CreateOrderDto(string Product, decimal TotalPrice);

    internal static IEndpointRouteBuilder MapApplicationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var usersGroup = endpoints.MapGroup("/");

        usersGroup.MapPost("create", CreateOrderAsync);

        usersGroup.MapGet("", GetOrdersAsync);

        return endpoints;
    }

    internal static async Task<Results<Ok<OrderDto>, Conflict<ErrorResponse>>> CreateOrderAsync
        (
            [FromHeader(Name = "X-Request-Id")] Guid RequestId,
            [FromBody] CreateOrderDto createOrderDto,
            OrderContext dbContext,
            IRequestManager requestManager,
            CancellationToken cancellationToken
        )
    {
        if (await requestManager.GetAsync(RequestId) is { } lastRequest)
        {
            if (Guid.TryParse(lastRequest.DataId, out var orderId))
            {
                if (await dbContext.Orders.FindAsync([orderId], cancellationToken) is { } order)
                {
                    if(order.Product != createOrderDto.Product ||
                       order.TotalPrice != createOrderDto.TotalPrice)
                    {
                        return TypedResults.Conflict(new ErrorResponse(
                            $"Запрос с id '{RequestId}' уже существует и данные в очередном запросе не совпадают с уже обрабатываемым запросом",
                            "IdempotentParameterMismatch"));
                    }
                }
                else
                {
                    // HTTP 500:
                    throw new InvalidOperationException($"Запрос с id '{RequestId}' уже существует, но отсутствуют данные по ранее созданному запросу");
                }
            }

            return TypedResults.Conflict(new ErrorResponse($"Запрос с id '{RequestId}' уже существует", "IdempotentError"));
        }

        var newOrder = new Order(createOrderDto.Product, createOrderDto.TotalPrice);
        var request = await requestManager.CreateRequestAsync<Order>(RequestId, newOrder.Id.ToString());

        dbContext.Orders.Add(newOrder);
        dbContext.ClientRequests.Add(request);
        await dbContext.SaveChangesAsync(cancellationToken);

        var orderResult = new OrderDto(newOrder.Id, newOrder.Product, newOrder.TotalPrice, newOrder.CreatedAt);

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
            .Select(order => new OrderDto(order.Id, order.Product, order.TotalPrice, order.CreatedAt))
            .ToListAsync(cancellationToken);

        return orders.Any()
            ? TypedResults.Ok(orders)
            : TypedResults.NotFound(new ErrorResponse("Заказы не существуют"));
    }
}
