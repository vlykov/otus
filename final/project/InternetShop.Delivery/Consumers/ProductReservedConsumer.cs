using MassTransit;
using InternetShop.Delivery.Domain;
using InternetShop.Delivery.Infrastructure.Persistence;
using static InternetShop.Common.Contracts.MessageBroker.Events.Delivery;
using static InternetShop.Common.Contracts.MessageBroker.Events.Warehouse;

namespace InternetShop.Delivery.Consumers;

public class ProductReservedConsumer(CoreDbContext dbContext) : IConsumer<ProductReserved>
{
    public async Task Consume(ConsumeContext<ProductReserved> context)
    {
        var orderId = context.Message.OrderId;
        var product = context.Message.Product;
        var cancellationToken = context.CancellationToken;

        try
        {
            var delivery = Domain.Delivery.Reserve(orderId, product);

            dbContext.Add(delivery);
            await dbContext.SaveChangesAsync(cancellationToken);

            await context.Publish(new DeliveryReserved(orderId), cancellationToken);
        }
        catch (Exception ex)
        {
            await context.Publish(new DeliveryReservationFailed(orderId, ex.Message), cancellationToken);
        }
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

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<ProductReservedConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(_ => _.Interval(5, 1000));

        // use the outbox to prevent duplicate events from being published
        //endpointConfigurator.UseInMemoryOutbox(context);
        endpointConfigurator.UseEntityFrameworkOutbox<CoreDbContext>(context);
    }
}
