using MassTransit;
using Microsoft.EntityFrameworkCore;
using Otus.Homework.Warehouse.Infrastructure.Persistence;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Delivery;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Warehouse;

namespace Otus.Homework.Warehouse.Consumers;

public class DeliveryReservationFailedConsumer(WarehouseContext dbContext) : IConsumer<DeliveryReservationFailed>
{
    public async Task Consume(ConsumeContext<DeliveryReservationFailed> context)
    {
        var orderId = context.Message.OrderId;
        var cancellationToken = context.CancellationToken;

        if (await dbContext.ProductReservations.FirstOrDefaultAsync(_ => _.OrderId == orderId, cancellationToken) is not { } productReservation)
        {
            throw new InvalidOperationException($"Заказ {orderId} в резервах склада не существует");
        }

        productReservation.Decline();
        await dbContext.SaveChangesAsync(cancellationToken);

        await context.Publish(new ProductReservationFailed(orderId), cancellationToken);
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

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<DeliveryReservationFailedConsumer> consumerConfigurator)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(_ => _.Interval(5, 1000));

        // use the outbox to prevent duplicate events from being published
        endpointConfigurator.UseInMemoryOutbox();
    }
}
