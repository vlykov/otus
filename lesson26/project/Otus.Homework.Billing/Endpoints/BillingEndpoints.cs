using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Otus.Homework.Billing.Infrastructure.Persistence;
using Otus.Homework.Common.Authentication.Models;
using Otus.Homework.Common.ExceptionHandlers.Models;

namespace Otus.Homework.Billing.Endpoints;

public static class BillingEndpoints
{
    internal record DepositDto(decimal Amount);
    internal record BalanceDto(decimal Balance);

    internal static IEndpointRouteBuilder MapApplicationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var usersGroup = endpoints.MapGroup("/balance");

        usersGroup.MapPost("/deposit", DepositAsync)
            .RequireAuthorization();
        usersGroup.MapGet("", GetBalanceAsync)
            .RequireAuthorization();

        return endpoints;
    }

    internal static async Task<Results<Ok, BadRequest<ErrorResponse>, NotFound<ErrorResponse>>> DepositAsync
        (
            [FromBody] DepositDto deposit,
            AccountContext dbContext,
            ClaimsPrincipal principal,
            CancellationToken cancellationToken
        )
    {
        var user = new User((ClaimsIdentity)principal.Identity!);

        if (await dbContext.Accounts.FirstOrDefaultAsync(_ => _.UserId == user.Id, cancellationToken) is not { } account)
        {
            return TypedResults.NotFound(new ErrorResponse("Аккаунт пользователя не существует"));
        }

        if (!account.ValidateDeposit(deposit.Amount))
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
        AccountContext dbContext,
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
