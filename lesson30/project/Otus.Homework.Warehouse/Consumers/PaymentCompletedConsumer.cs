using MassTransit;
using Otus.Homework.Warehouse.Domain;
using Otus.Homework.Warehouse.Infrastructure.Persistence;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Billing;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Warehouse;

namespace Otus.Homework.Warehouse.Consumers;

public class PaymentCompletedConsumer(WarehouseContext dbContext) : IConsumer<PaymentCompleted>
{
    public async Task Consume(ConsumeContext<PaymentCompleted> context)
    {
        var orderId = context.Message.OrderId;
        var product = context.Message.Product;
        var cancellationToken = context.CancellationToken;

        if (product.Equals("Notexists")) //for testing purpose
        {
            await context.Publish(new ProductReservationFailed(orderId), cancellationToken);

            return;
        }

        var payment = Reservation.Reserve(orderId, product);

        dbContext.Add(payment);
        await dbContext.SaveChangesAsync(cancellationToken);

        await context.Publish(new ProductReserved(orderId, product), cancellationToken);
    }
}

public class PaymentCompletedConsumerDefinition : ConsumerDefinition<PaymentCompletedConsumer>
{
    public PaymentCompletedConsumerDefinition()
    {
        // override the default endpoint name
        //EndpointName = EndpointNames.OrdersEndpoint;

        //// limit the number of messages consumed concurrently
        //// this applies to the consumer only, not the endpoint
        //ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<PaymentCompletedConsumer> consumerConfigurator)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(_ => _.Interval(5, 1000));

        // use the outbox to prevent duplicate events from being published
        endpointConfigurator.UseInMemoryOutbox();
    }
}
