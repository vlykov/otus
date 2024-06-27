using MassTransit;
using InternetShop.Delivery.Domain;
using InternetShop.Delivery.Infrastructure.Persistence;
using static InternetShop.Common.Contracts.MessageBroker.Events.Delivery;
using static InternetShop.Common.Contracts.MessageBroker.Events.Warehouse;

namespace InternetShop.Delivery.Consumers;

public class ProductsReservedConsumer(CoreDbContext dbContext) : IConsumer<ProductsReserved>
{
    public async Task Consume(ConsumeContext<ProductsReserved> context)
    {
        var orderId = context.Message.OrderId;
        var cancellationToken = context.CancellationToken;

        // todo: в этот сервис полезно было бы передавать габариты товаров

        try
        {
            var delivery = Domain.Delivery.Reserve(orderId);

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

public class ProductsReservedConsumerDefinition : ConsumerDefinition<ProductsReservedConsumer>
{
    public ProductsReservedConsumerDefinition()
    {
        // override the default endpoint name
        //EndpointName = EndpointNames.OrdersEndpoint;

        //// limit the number of messages consumed concurrently
        //// this applies to the consumer only, not the endpoint
        //ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<ProductsReservedConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(_ => _.Interval(5, 1000));

        // use the outbox to prevent duplicate events from being published
        //endpointConfigurator.UseInMemoryOutbox(context);
        endpointConfigurator.UseEntityFrameworkOutbox<CoreDbContext>(context);
    }
}
