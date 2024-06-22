using MassTransit;
using Microsoft.EntityFrameworkCore;
using Otus.Homework.Billing.Infrastructure.Persistence;
using Otus.Homework.Common.RabbitMq.Constants;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Billing;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Orders;

namespace Otus.Homework.Billing.Consumers;

public class OrderCreatedConsumer(AccountContext dbContext) : IConsumer<OrderCreated>
{
    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        var userId = context.Message.UserId;
        var orderId = context.Message.OrderId;
        var totalPrice = context.Message.TotalPrice;
        var cancellationToken = context.CancellationToken;

        if (await dbContext.Accounts.FirstOrDefaultAsync(_ => _.UserId == userId, cancellationToken) is not { } account)
        {
            throw new InvalidOperationException("Аккаунт пользователя не существует");
        }

        if (!account.CheckSufficientBalance(totalPrice))
        {
            //await context.Publish(new OrderUnpaid(orderId), _ => _.SetRoutingKey(""), cancellationToken);
            await context.Publish(new OrderUnpaid(orderId), cancellationToken);

            return;
        }

        account.Withdraw(totalPrice);
        await dbContext.SaveChangesAsync(cancellationToken);

        await context.Publish(new OrderPaid(orderId), cancellationToken);
    }
}

public class OrderCreatedConsumerDefinition :  ConsumerDefinition<OrderCreatedConsumer>
{
    public OrderCreatedConsumerDefinition()
    {
        // override the default endpoint name
        EndpointName = EndpointNames.OrdersEndpoint;

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
