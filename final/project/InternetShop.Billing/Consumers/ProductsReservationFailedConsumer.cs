using MassTransit;
using Microsoft.EntityFrameworkCore;
using InternetShop.Billing.Infrastructure.Persistence;
using static InternetShop.Common.Contracts.MessageBroker.Events.Billing;
using static InternetShop.Common.Contracts.MessageBroker.Events.Warehouse;
using InternetShop.Billing.Domain;
namespace InternetShop.Billing.Consumers;

public class ProductsReservationFailedConsumer(CoreDbContext dbContext) : IConsumer<ProductsReservationFailed>
{
    public async Task Consume(ConsumeContext<ProductsReservationFailed> context)
    {
        var orderId = context.Message.OrderId;
        var reason = context.Message.Reason;
        var cancellationToken = context.CancellationToken;

        if (await dbContext.Payments
            .OrderByDescending(_ => _.CreatedAt)
            .FirstOrDefaultAsync(_ => _.OrderId == orderId, cancellationToken)
            is not { } payment)
        {
            throw new InvalidOperationException($"Платёж по заказу {orderId} в биллинге не существует");
        }

        if (await dbContext.Accounts.FirstOrDefaultAsync(_ => _.UserId == payment.UserId, cancellationToken) is not { } account)
        {
            throw new InvalidOperationException("Аккаунт пользователя не существует");
        }

        account.Deposit(payment.TotalPrice);

        var rollbackPayment = Payment.Charge(orderId, payment.UserId, payment.TotalPrice);
        rollbackPayment.ReturnMoney();
        dbContext.Add(rollbackPayment);

        await dbContext.SaveChangesAsync(cancellationToken);

        await context.Publish(new PaymentFailed(orderId, reason), cancellationToken);
    }
}

public class ProductsReservationFailedConsumerDefinition :  ConsumerDefinition<ProductsReservationFailedConsumer>
{
    public ProductsReservationFailedConsumerDefinition()
    {
        // override the default endpoint name
        //EndpointName = EndpointNames.OrdersEndpoint;

        //// limit the number of messages consumed concurrently
        //// this applies to the consumer only, not the endpoint
        //ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<ProductsReservationFailedConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(_ => _.Interval(5, 1000));

        // use the outbox to prevent duplicate events from being published
        //endpointConfigurator.UseInMemoryOutbox(context);
        endpointConfigurator.UseEntityFrameworkOutbox<CoreDbContext>(context);
    }
}
