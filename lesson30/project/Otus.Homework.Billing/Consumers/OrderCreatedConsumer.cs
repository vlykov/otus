using MassTransit;
using Otus.Homework.Billing.Domain;
using Otus.Homework.Billing.Infrastructure.Persistence;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Billing;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Orders;

namespace Otus.Homework.Billing.Consumers;

public class OrderCreatedConsumer(PaymentContext dbContext) : IConsumer<OrderCreated>
{
    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        var orderId = context.Message.OrderId;
        var product = context.Message.Product;
        var totalPrice = context.Message.TotalPrice;
        var cancellationToken = context.CancellationToken;

        if (product.Equals("Expensive")) //for testing purpose
        {
            await context.Publish(new PaymentFailed(orderId), cancellationToken);

            return;
        }

        var payment = Payment.Charge(orderId, product, totalPrice);

        dbContext.Add(payment);
        await dbContext.SaveChangesAsync(cancellationToken);

        await context.Publish(new PaymentCompleted(orderId, product), cancellationToken);
    }
}

public class OrderCreatedConsumerDefinition :  ConsumerDefinition<OrderCreatedConsumer>
{
    public OrderCreatedConsumerDefinition()
    {
        // override the default endpoint name
        //EndpointName = EndpointNames.OrdersEndpoint;

        //// limit the number of messages consumed concurrently
        //// this applies to the consumer only, not the endpoint
        //ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<OrderCreatedConsumer> consumerConfigurator)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(_ => _.Interval(5, 1000));

        // use the outbox to prevent duplicate events from being published
        endpointConfigurator.UseInMemoryOutbox();
    }
}
