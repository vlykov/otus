using MassTransit;
using Otus.Homework.Orders.Infrastructure.Persistence;
using Otus.Homework.Common.RabbitMq.Constants;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Billing;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Orders;

namespace Otus.Homework.Orders.Consumers;

public class OrderPaidConsumer(OrderContext dbContext) : IConsumer<OrderPaid>
{
    public async Task Consume(ConsumeContext<OrderPaid> context)
    {
        var orderId = context.Message.OrderId;
        var cancellationToken = context.CancellationToken;

        if (await dbContext.Orders.FindAsync([orderId], cancellationToken) is not { } order)
        {
            throw new InvalidOperationException("Заказ не существует");
        }

        order.SetConfirmed();

        //await context.Publish(new OrderConfirmed(orderId, order.UserId), _ => _.SetRoutingKey(""), cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        await context.Publish(new OrderConfirmed(orderId, order.UserId), cancellationToken);
    }
}

public class OrderPaidConsumerDefinition :  ConsumerDefinition<OrderPaidConsumer>
{
    public OrderPaidConsumerDefinition()
    {
        // override the default endpoint name
        EndpointName = EndpointNames.BillingEndpoint;

        //// limit the number of messages consumed concurrently
        //// this applies to the consumer only, not the endpoint
        //ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<OrderPaidConsumer> consumerConfigurator)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(_ => _.Interval(5, 1000));

        // use the outbox to prevent duplicate events from being published
        endpointConfigurator.UseInMemoryOutbox();
    }
}
