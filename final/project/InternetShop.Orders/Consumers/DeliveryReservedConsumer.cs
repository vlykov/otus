using MassTransit;
using InternetShop.Orders.Infrastructure.Persistence;
using static InternetShop.Common.Contracts.MessageBroker.Events.Delivery;
using static InternetShop.Common.Contracts.MessageBroker.Events.Orders;

namespace InternetShop.Orders.Consumers;

public class DeliveryReservedConsumer(CoreDbContext dbContext) : IConsumer<DeliveryReserved>
{
    public async Task Consume(ConsumeContext<DeliveryReserved> context)
    {
        var orderId = context.Message.OrderId;
        var cancellationToken = context.CancellationToken;

        if (await dbContext.Orders.FindAsync([orderId], cancellationToken) is not { } order)
        {
            throw new InvalidOperationException("Заказ не существует");
        }

        order.SetConfirmed();

        await dbContext.SaveChangesAsync(cancellationToken);

        await context.Publish(new OrderConfirmed(orderId, order.UserId), cancellationToken);
    }
}

public class DeliveryReservedConsumerDefinition :  ConsumerDefinition<DeliveryReservedConsumer>
{
    public DeliveryReservedConsumerDefinition()
    {
        // override the default endpoint name
        //EndpointName = EndpointNames.BillingEndpoint;

        //// limit the number of messages consumed concurrently
        //// this applies to the consumer only, not the endpoint
        //ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<DeliveryReservedConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(_ => _.Interval(5, 1000));

        // use the outbox to prevent duplicate events from being published
        //endpointConfigurator.UseInMemoryOutbox(context);
        endpointConfigurator.UseEntityFrameworkOutbox<CoreDbContext>(context);
    }
}
