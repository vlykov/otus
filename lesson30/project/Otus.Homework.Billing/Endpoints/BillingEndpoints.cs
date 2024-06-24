using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Otus.Homework.Billing.Infrastructure.Persistence;
using Otus.Homework.Common.ExceptionHandlers.Models;

namespace Otus.Homework.Billing.Endpoints;

public static class BillingEndpoints
{
    internal record PaymentDto(int Id, int OrderId, string Product, decimal TotalPrice, string Status, DateTimeOffset CreatedAt);

    internal static IEndpointRouteBuilder MapApplicationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var usersGroup = endpoints.MapGroup("/payments");

        usersGroup.MapGet("", GetPaymentsAsync);

        return endpoints;
    }

    internal static async Task<Results<Ok<List<PaymentDto>>, NotFound<ErrorResponse>>> GetPaymentsAsync
    (
        PaymentContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var payments = await dbContext.Payments
            .AsNoTracking()
            .OrderByDescending(_ => _.CreatedAt)
            .Select(payment => new PaymentDto(payment.Id, payment.OrderId, payment.Product, payment.TotalPrice, payment.Status, payment.CreatedAt))
            .ToListAsync(cancellationToken);

        return payments.Any()
            ? TypedResults.Ok(payments)
            : TypedResults.NotFound(new ErrorResponse("Платежи не существуют"));
    }
}
