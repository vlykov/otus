using System.Security.Claims;
using System.Threading;
using InternetShop.Common.Authentication.Models;
using InternetShop.Common.ExceptionHandlers.Models;
using InternetShop.Warehouse.Infrastructure.Persistence;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternetShop.Warehouse.Endpoints;

public static class ProductEndpoints
{
    internal record SetQuantityDto(int ProductId, int Quantity);
    internal record ProductDto(int Id, string Name, int Quantity, DateTimeOffset CreatedAt);

    internal static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var usersGroup = endpoints.MapGroup("/product")
            .RequireAuthorization();

        usersGroup.MapPost("/set-quantity", SetQuantityAsync);
        usersGroup.MapGet("", GetProductsAsync);

        return endpoints;
    }

    internal static async Task<Results<Ok, BadRequest<ErrorResponse>, NotFound<ErrorResponse>>> SetQuantityAsync
    (
        [FromBody] SetQuantityDto request,
        CoreDbContext dbContext,
        ClaimsPrincipal principal,
        CancellationToken cancellationToken
    )
    {
        //var user = new User((ClaimsIdentity)principal.Identity!);

        if (await dbContext.Products.FindAsync([request.ProductId], cancellationToken) is not { } product)
        {
            return TypedResults.NotFound(new ErrorResponse($"Продукт с id {request.ProductId} не существует"));
        }

        product.SetQuantity(request.Quantity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok();
    }

    internal static async Task<Results<Ok<List<ProductDto>>, NotFound<ErrorResponse>>> GetProductsAsync
    (
        CoreDbContext dbContext,
        CancellationToken cancellationToken
    )
    {        
        var products = await dbContext.Products
            .AsNoTracking()
            .OrderByDescending(_ => _.Name)
            .Select(product => new ProductDto(product.Id, product.Name, product.Quantity, product.CreatedAt))
            .ToListAsync(cancellationToken);

        return products.Count == 0
            ? TypedResults.NotFound(new ErrorResponse("Товары не существуют"))
            : TypedResults.Ok(products);
    }
}
