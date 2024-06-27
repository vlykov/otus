using MassTransit;
using Microsoft.EntityFrameworkCore;
using InternetShop.Warehouse.Infrastructure.Persistence;
using static InternetShop.Common.Contracts.MessageBroker.Events.Delivery;
using static InternetShop.Common.Contracts.MessageBroker.Events.Warehouse;

namespace InternetShop.Warehouse.Consumers;

public class DeliveryReservationFailedConsumer(CoreDbContext dbContext) : IConsumer<DeliveryReservationFailed>
{
    public async Task Consume(ConsumeContext<DeliveryReservationFailed> context)
    {
        var orderId = context.Message.OrderId;
        var reason = context.Message.Reason;
        var cancellationToken = context.CancellationToken;

        if (await dbContext.ProductReservations
            .OrderByDescending(_ => _.CreatedAt)
            .FirstOrDefaultAsync(_ => _.OrderId == orderId, cancellationToken) is not { } productReservation)
        {
            throw new InvalidOperationException($"Заказ {orderId} в резервах склада не существует");
        }
        if (await dbContext.Products.FirstOrDefaultAsync(_ => _.Id == productReservation.ProductId, cancellationToken) is not { } product)
        {
            throw new InvalidOperationException("Товар не существует");
        }

        product.IncreaseQuantity(productReservation.Quantity);
        productReservation.Decline();
        await dbContext.SaveChangesAsync(cancellationToken);

        await context.Publish(new ProductsReservationFailed(orderId, reason), cancellationToken);
    }
}

public class DeliveryReservationFailedConsumerDefinition : ConsumerDefinition<DeliveryReservationFailedConsumer>
{
    public DeliveryReservationFailedConsumerDefinition()
    {
        // override the default endpoint name
        //EndpointName = EndpointNames.OrdersEndpoint;

        //// limit the number of messages consumed concurrently
        //// this applies to the consumer only, not the endpoint
        //ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<DeliveryReservationFailedConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(_ => _.Interval(5, 1000));

        // use the outbox to prevent duplicate events from being published
        //endpointConfigurator.UseInMemoryOutbox(context);
        endpointConfigurator.UseEntityFrameworkOutbox<CoreDbContext>(context);
    }
}
