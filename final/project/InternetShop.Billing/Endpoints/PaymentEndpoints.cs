using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using InternetShop.Common.ExceptionHandlers.Models;
using InternetShop.Billing.Infrastructure.Persistence;
using System.Security.Claims;

namespace InternetShop.Billing.Endpoints;

public static class PaymentEndpoints
{
    internal record PaymentDto(int Id, int UserId, int OrderId, decimal TotalPrice, string Status, DateTimeOffset CreatedAt);

    internal static IEndpointRouteBuilder MapPaymentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var usersGroup = endpoints.MapGroup("/payments")
            .RequireAuthorization();

        usersGroup.MapGet("", GetPaymentsAsync);

        return endpoints;
    }

    internal static async Task<Results<Ok<List<PaymentDto>>, NotFound<ErrorResponse>>> GetPaymentsAsync
    (
        CoreDbContext dbContext,
        ClaimsPrincipal principal,
        CancellationToken cancellationToken
    )
    {
        //var user = new User((ClaimsIdentity)principal.Identity!);

        var payments = await dbContext.Payments
            .AsNoTracking()
            .OrderByDescending(_ => _.CreatedAt)
            .Select(payment => new PaymentDto(payment.Id, payment.UserId, payment.OrderId, payment.TotalPrice, payment.Status, payment.CreatedAt))
            .ToListAsync(cancellationToken);

        return payments.Count == 0
            ? TypedResults.NotFound(new ErrorResponse("Платежи не существуют"))
            : TypedResults.Ok(payments);
    }
}
