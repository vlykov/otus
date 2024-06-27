using System.Security.Claims;
using InternetShop.Billing.Infrastructure.Persistence;
using InternetShop.Common.Authentication.Models;
using InternetShop.Common.ExceptionHandlers.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternetShop.Billing.Endpoints;

public static class BalanceEndpoints
{
    internal record DepositDto(decimal Amount);
    internal record BalanceDto(decimal Balance);

    internal static IEndpointRouteBuilder MapBalanceEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var usersGroup = endpoints.MapGroup("/billing/balance")
            .RequireAuthorization();

        usersGroup.MapPost("/deposit", DepositAsync);
        usersGroup.MapGet("", GetBalanceAsync);

        return endpoints;
    }

    internal static async Task<Results<Ok, BadRequest<ErrorResponse>, NotFound<ErrorResponse>>> DepositAsync
    (
        [FromBody] DepositDto deposit,
        CoreDbContext dbContext,
        ClaimsPrincipal principal,
        CancellationToken cancellationToken
    )
    {
        var user = new User((ClaimsIdentity)principal.Identity!);

        if (await dbContext.Accounts.FirstOrDefaultAsync(_ => _.UserId == user.Id, cancellationToken) is not { } account)
        {
            return TypedResults.NotFound(new ErrorResponse("Аккаунт пользователя не существует"));
        }

        if (!account.CheckIsValidDeposit(deposit.Amount))
        {
            return TypedResults.BadRequest(new ErrorResponse("Некорректная сумма внесения"));
        }

        account.Deposit(deposit.Amount);

        await dbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok();
    }

    internal static async Task<Results<Ok<BalanceDto>, NotFound<ErrorResponse>>> GetBalanceAsync
    (
        ClaimsPrincipal principal,
        CoreDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var user = new User((ClaimsIdentity)principal.Identity!);

        if (await dbContext.Accounts.AsNoTracking().FirstOrDefaultAsync(_ => _.UserId == user.Id, cancellationToken) is not { } account)
        {
            return TypedResults.NotFound(new ErrorResponse("Аккаунт пользователя не существует"));
        }

        return TypedResults.Ok(new BalanceDto(account.Balance));
    }
}
