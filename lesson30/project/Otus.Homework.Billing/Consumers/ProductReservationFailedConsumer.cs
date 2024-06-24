using MassTransit;
using Microsoft.EntityFrameworkCore;
using Otus.Homework.Billing.Infrastructure.Persistence;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Billing;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Warehouse;
namespace Otus.Homework.Billing.Consumers;

public class ProductReservationFailedConsumer(PaymentContext dbContext) : IConsumer<ProductReservationFailed>
{
    public async Task Consume(ConsumeContext<ProductReservationFailed> context)
    {
        var orderId = context.Message.OrderId;
        var cancellationToken = context.CancellationToken;

        if (await dbContext.Payments.FirstOrDefaultAsync(_ => _.OrderId == orderId, cancellationToken) is not { } payment)
        {
            throw new InvalidOperationException($"Заказ {orderId} в биллинге не существует");
        }

        payment.ReturnMoney();
        await dbContext.SaveChangesAsync(cancellationToken);

        await context.Publish(new PaymentFailed(orderId), cancellationToken);
    }
}

public class ProductsReservationFailedConsumerDefinition :  ConsumerDefinition<ProductReservationFailedConsumer>
{
    public ProductsReservationFailedConsumerDefinition()
    {
        // override the default endpoint name
        //EndpointName = EndpointNames.OrdersEndpoint;

        //// limit the number of messages consumed concurrently
        //// this applies to the consumer only, not the endpoint
        //ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<ProductReservationFailedConsumer> consumerConfigurator)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(_ => _.Interval(5, 1000));

        // use the outbox to prevent duplicate events from being published
        endpointConfigurator.UseInMemoryOutbox();
    }
}
