using MassTransit;
using InternetShop.Orders.Infrastructure.Persistence;
using static InternetShop.Common.Contracts.MessageBroker.Events.Delivery;

namespace InternetShop.Orders.Consumers;

public class DeliveryCompletedConsumer(CoreDbContext dbContext) : IConsumer<DeliveryCompleted>
{
    public async Task Consume(ConsumeContext<DeliveryCompleted> context)
    {
        var orderId = context.Message.OrderId;
        var cancellationToken = context.CancellationToken;

        if (await dbContext.Orders.FindAsync([orderId], cancellationToken) is not { } order)
        {
            throw new InvalidOperationException("Заказ не существует");
        }

        order.SetCompleted();

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

public class DeliveryCompletedConsumerDefinition :  ConsumerDefinition<DeliveryCompletedConsumer>
{
    public DeliveryCompletedConsumerDefinition()
    {
        // override the default endpoint name
        //EndpointName = EndpointNames.BillingEndpoint;

        //// limit the number of messages consumed concurrently
        //// this applies to the consumer only, not the endpoint
        //ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<DeliveryCompletedConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(_ => _.Interval(5, 1000));

        // use the outbox to prevent duplicate events from being published
        //endpointConfigurator.UseInMemoryOutbox(context);
        endpointConfigurator.UseEntityFrameworkOutbox<CoreDbContext>(context);
    }
}
