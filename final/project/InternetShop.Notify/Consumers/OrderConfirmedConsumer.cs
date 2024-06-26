using MassTransit;
using InternetShop.Notify.Domain;
using InternetShop.Notify.Infrastructure.Persistence;
using InternetShop.Common.RabbitMq.Constants;
using static InternetShop.Common.Contracts.MessageBroker.Events.Orders;

namespace InternetShop.Notify.Consumers;

public class OrderConfirmedConsumer(CoreDbContext dbContext) : IConsumer<OrderConfirmed>
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

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<OrderConfirmedConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(_ => _.Interval(5, 1000));

        // use the outbox to prevent duplicate events from being published
        //endpointConfigurator.UseInMemoryOutbox(context);
        endpointConfigurator.UseEntityFrameworkOutbox<CoreDbContext>(context);
    }
}
