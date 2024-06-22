using MassTransit;
using Otus.Homework.Notify.Domain;
using Otus.Homework.Notify.Infrastructure.Persistence;
using Otus.Homework.Common.RabbitMq.Constants;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Orders;

namespace Otus.Homework.Notify.Consumers;

public class OrderConfirmedConsumer(NotifyContext dbContext) : IConsumer<OrderConfirmed>
{
    public async Task Consume(ConsumeContext<OrderConfirmed> context)
    {
        var userId = context.Message.UserId;
        var orderId = context.Message.OrderId;
        var cancellationToken = context.CancellationToken;

        var notification = new Notification(userId, $"Congratulations! Order '{orderId}' is confirmed for user '{userId}'");

        dbContext.Notifications.Add(notification);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

public class OrderConfirmedConsumerDefinition : ConsumerDefinition<OrderConfirmedConsumer>
{
    public OrderConfirmedConsumerDefinition()
    {
        // override the default endpoint name
        EndpointName = EndpointNames.OrdersEndpoint;

        //// limit the number of messages consumed concurrently
        //// this applies to the consumer only, not the endpoint
        //ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<OrderConfirmedConsumer> consumerConfigurator)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(_ => _.Interval(5, 1000));

        // use the outbox to prevent duplicate events from being published
        endpointConfigurator.UseInMemoryOutbox();
    }
}
