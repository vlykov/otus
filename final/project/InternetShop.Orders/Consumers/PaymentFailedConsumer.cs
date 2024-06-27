using MassTransit;
using InternetShop.Orders.Infrastructure.Persistence;
using static InternetShop.Common.Contracts.MessageBroker.Events.Billing;
using static InternetShop.Common.Contracts.MessageBroker.Events.Orders;

namespace InternetShop.Orders.Consumers;

public class PaymentFailedConsumer(CoreDbContext dbContext) : IConsumer<PaymentFailed>
{
    public async Task Consume(ConsumeContext<PaymentFailed> context)
    {
        var orderId = context.Message.OrderId;
        var reason = context.Message.Reason;
        var cancellationToken = context.CancellationToken;

        if (await dbContext.Orders.FindAsync([orderId], cancellationToken) is not { } order)
        {
            throw new InvalidOperationException("Заказ не существует");
        }

        order.SetDeclined(reason);

        await dbContext.SaveChangesAsync(cancellationToken);

        await context.Publish(new OrderUnconfirmed(orderId, order.UserId, reason), cancellationToken);
    }
}

public class PaymentFailedConsumerDefinition :  ConsumerDefinition<PaymentFailedConsumer>
{
    public PaymentFailedConsumerDefinition()
    {
        // override the default endpoint name
        //EndpointName = EndpointNames.BillingEndpoint;

        //// limit the number of messages consumed concurrently
        //// this applies to the consumer only, not the endpoint
        //ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<PaymentFailedConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(_ => _.Interval(5, 1000));

        // use the outbox to prevent duplicate events from being published
        //endpointConfigurator.UseInMemoryOutbox(context);
        endpointConfigurator.UseEntityFrameworkOutbox<CoreDbContext>(context);
    }
}
