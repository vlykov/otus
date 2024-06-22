using MassTransit;
using Otus.Homework.Notify.Domain;
using Otus.Homework.Notify.Infrastructure.Persistence;
using Otus.Homework.Common.RabbitMq.Constants;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Orders;

namespace Otus.Homework.Notify.Consumers;

public class OrderUnconfirmedConsumer(NotifyContext dbContext) : IConsumer<OrderUnconfirmed>
{
    public async Task Consume(ConsumeContext<OrderUnconfirmed> context)
    {
        var userId = context.Message.UserId;
        var orderId = context.Message.OrderId;
        var cancellationToken = context.CancellationToken;

        var notification = new Notification(userId, $"Sorry. Order '{orderId}' is not confirmed for user '{userId}'");

        dbContext.Notifications.Add(notification);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

public class OrderUnconfirmedConsumerDefinition : ConsumerDefinition<OrderUnconfirmedConsumer>
{
    public OrderUnconfirmedConsumerDefinition()
    {
        // override the default endpoint name
        EndpointName = EndpointNames.OrdersEndpoint;

        //// limit the number of messages consumed concurrently
        //// this applies to the consumer only, not the endpoint
        //ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<OrderUnconfirmedConsumer> consumerConfigurator)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(_ => _.Interval(5, 1000));

        // use the outbox to prevent duplicate events from being published
        endpointConfigurator.UseInMemoryOutbox();
    }
}
