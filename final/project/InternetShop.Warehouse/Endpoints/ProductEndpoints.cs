using System.Security.Claims;
using InternetShop.Common.Authentication.Models;
using InternetShop.Common.ExceptionHandlers.Models;
using InternetShop.Warehouse.Domain;
using InternetShop.Warehouse.Infrastructure.Persistence;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static InternetShop.Common.Contracts.MessageBroker.Events.Identity;

namespace InternetShop.Warehouse.Endpoints;

public static class ProductEndpoints
{
    internal record SetQuantityDto(int ProductId, int Quantity);
    internal record SetPriceDto(int ProductId, decimal Price);
    internal record CreateProductDto(string Name, int Quantity, decimal Price);
    internal record ProductDto(int ProductId, string ProductName, int Quantity, decimal Price);

    internal static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var usersGroup = endpoints.MapGroup("/products")
            .RequireAuthorization();

        usersGroup.MapPost("/quantity", ChangeQuantityAsync);
        usersGroup.MapPost("/price", ChangePriceAsync);
        usersGroup.MapPost("", AddProductAsync);
        usersGroup.MapGet("", GetProductsAsync);
        usersGroup.MapGet("/search", SearchProductsAsync);

        return endpoints;
    }

    internal static async Task<Results<Ok<ProductDto>, BadRequest<ErrorResponse>>> AddProductAsync
        (
            [FromBody] CreateProductDto productDto,
            CoreDbContext dbContext,
            IBus bus,
            ClaimsPrincipal principal,
            CancellationToken cancellationToken
        )
    {
        //var user = new User((ClaimsIdentity)principal.Identity!);

        if (await dbContext.Products.AnyAsync(product => product.Name == productDto.Name, cancellationToken))
        {
            return TypedResults.BadRequest(new ErrorResponse("Товар уже существует"));
        }

        var product = Product.Create(productDto.Name);
        product.SetQuantity(productDto.Quantity);
        product.SetPrice(productDto.Price);

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        var newProduct = new ProductDto(product.Id, product.Name, product.Quantity, product.Price);

        return TypedResults.Ok(newProduct);
    }

    internal static async Task<Results<Ok, BadRequest<ErrorResponse>, NotFound<ErrorResponse>>> ChangeQuantityAsync
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

    internal static async Task<Results<Ok, BadRequest<ErrorResponse>, NotFound<ErrorResponse>>> ChangePriceAsync
    (
        [FromBody] SetPriceDto request,
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

        product.SetPrice(request.Price);

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
            .Select(product => new ProductDto(product.Id, product.Name, product.Quantity, product.Price))
            .ToListAsync(cancellationToken);

        var a = dbContext.Products.Where(p => EF.Functions.Like(p.Name!, "%Tom%"));

        return products.Count == 0
            ? TypedResults.NotFound(new ErrorResponse("Товары не существуют"))
            : TypedResults.Ok(products);
    }

    internal static async Task<Results<Ok<List<ProductDto>>, NotFound<ErrorResponse>>> SearchProductsAsync
    (
        [FromQuery] string term,
        CoreDbContext dbContext,
        CancellationToken cancellationToken
    )
    {        
        var products = await dbContext.Products
            .AsNoTracking()
            .Where(p => EF.Functions.Like(p.Name!, $"%{term}%"))
            .OrderBy(_ => _.Name)
            .Select(product => new ProductDto(product.Id, product.Name, product.Quantity, product.Price))
            .ToListAsync(cancellationToken);

        return products.Count == 0
            ? TypedResults.NotFound(new ErrorResponse("Товары не существуют"))
            : TypedResults.Ok(products);
    }
}
