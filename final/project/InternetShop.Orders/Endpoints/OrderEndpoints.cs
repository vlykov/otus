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
using InternetShop.Common.Contracts.MessageBroker;

namespace InternetShop.Orders.Endpoints;

public static class OrderEndpoints
{
    internal record OrderPositionDto (int ProductId, string ProductName, int Quantity, decimal Price);
    internal record OrderDto(int Id, string Status, decimal TotalPrice, ICollection<OrderPositionDto> Positions, string? Reason, DateTimeOffset CreatedAt);
    internal record CreateOrderDto(ICollection<OrderPositionDto> Positions);

    internal static IEndpointRouteBuilder MapApplicationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var usersGroup = endpoints.MapGroup("/orders")
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
        if (createOrderDto.Positions == null || createOrderDto.Positions.Any(_ => _.Quantity < 1))
        {
            return TypedResults.BadRequest(new ErrorResponse($"Неправильное количество товаров"));
        }

        if (await requestManager.ExistAsync(RequestId))
        {
            return TypedResults.Conflict(new ErrorResponse($"Запрос с id '{RequestId}' уже существует", "IdempotentError"));
        }

        var user = new User((ClaimsIdentity)principal.Identity!);

        var order = new Order(user.Id, createOrderDto.Positions
            .Select(_ => new OrderPosition(_.ProductId, _.ProductName, _.Quantity, _.Price))
            .ToList());
        var positions = order.Positions.Select(_ => new Models.Orders.OrderPosition(_.ProductId, _.Name, _.Quantity, _.Price)).ToList();

        var request = await requestManager.CreateRequestAsync<Order>(RequestId, order.Id.ToString());

        dbContext.Orders.Add(order);
        dbContext.ClientRequests.Add(request);
        await dbContext.SaveChangesAsync(cancellationToken);

        await bus.Publish(new OrderCreated(order.Id, user.Id, positions, order.TotalPrice), cancellationToken);

        var orderResult = new OrderDto(order.Id, order.Status, order.TotalPrice, order.Positions.Select(Map).ToList(), order.Reason, order.CreatedAt);

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
            .Include(_ => _.Positions)
            .OrderByDescending(_ => _.CreatedAt)
            .ToListAsync();

        var result = orders
            .Select(order => new OrderDto(order.Id, order.Status, order.TotalPrice, order.Positions.Select(Map).ToList(), order.Reason, order.CreatedAt))
            .ToList();

        return result.Count == 0
            ? TypedResults.NotFound(new ErrorResponse("Заказы не существуют"))
            : TypedResults.Ok(result);
    }

    private static OrderPositionDto Map(OrderPosition position)
    {
        return new OrderPositionDto(position.ProductId, position.Name, position.Quantity, position.Price);
    }
}
