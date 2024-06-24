using MassTransit;
using Otus.Homework.Orders.Infrastructure.Persistence;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Billing;

namespace Otus.Homework.Orders.Consumers;

public class PaymentFailedConsumer(OrderContext dbContext) : IConsumer<PaymentFailed>
{
    public async Task Consume(ConsumeContext<PaymentFailed> context)
    {
        var orderId = context.Message.OrderId;
        var cancellationToken = context.CancellationToken;

        if (await dbContext.Orders.FindAsync([orderId], cancellationToken) is not { } order)
        {
            throw new InvalidOperationException("Заказ не существует");
        }

        order.SetDeclined();

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

public class PaymentFailedConsumerDefinition :  ConsumerDefinition<PaymentFailedConsumer>
{
    public PaymentFailedConsumerDefinition()
    {
        // override the default endpoint name
        //EndpointName = EndpointNames.BillingEndpoint;

        //// limit the number of messages consumed concurrently
        //// this applies to the consumer only, not the endpoint
        //ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<PaymentFailedConsumer> consumerConfigurator)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(_ => _.Interval(5, 1000));

        // use the outbox to prevent duplicate events from being published
        endpointConfigurator.UseInMemoryOutbox();
    }
}
