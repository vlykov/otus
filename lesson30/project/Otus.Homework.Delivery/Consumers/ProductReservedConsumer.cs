using MassTransit;
using Otus.Homework.Delivery.Domain;
using Otus.Homework.Delivery.Infrastructure.Persistence;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Delivery;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Warehouse;

namespace Otus.Homework.Delivery.Consumers;

public class ProductReservedConsumer(DeliveryContext dbContext) : IConsumer<ProductReserved>
{
    public async Task Consume(ConsumeContext<ProductReserved> context)
    {
        var orderId = context.Message.OrderId;
        var product = context.Message.Product;
        var cancellationToken = context.CancellationToken;

        if (product.Equals("LargeSized")) //for testing purpose
        {
            await context.Publish(new DeliveryReservationFailed(orderId), cancellationToken);

            return;
        }

        var delivery = Courier.Reserve(orderId, product);

        dbContext.Add(delivery);
        await dbContext.SaveChangesAsync(cancellationToken);

        await context.Publish(new DeliveryReserved(orderId), cancellationToken);
    }
}

public class ProductReservedConsumerDefinition : ConsumerDefinition<ProductReservedConsumer>
{
    public ProductReservedConsumerDefinition()
    {
        // override the default endpoint name
        //EndpointName = EndpointNames.OrdersEndpoint;

        //// limit the number of messages consumed concurrently
        //// this applies to the consumer only, not the endpoint
        //ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<ProductReservedConsumer> consumerConfigurator)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(_ => _.Interval(5, 1000));

        // use the outbox to prevent duplicate events from being published
        endpointConfigurator.UseInMemoryOutbox();
    }
}
