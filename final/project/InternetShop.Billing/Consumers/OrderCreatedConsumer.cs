using System;
using InternetShop.Billing.Domain;
using InternetShop.Billing.Infrastructure.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using static InternetShop.Common.Contracts.MessageBroker.Events.Billing;
using static InternetShop.Common.Contracts.MessageBroker.Events.Orders;

namespace InternetShop.Billing.Consumers;

public class OrderCreatedConsumer(CoreDbContext dbContext) : IConsumer<OrderCreated>
{
    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        var userId = context.Message.UserId;
        var orderId = context.Message.OrderId;
        var products = context.Message.Products;
        var totalPrice = context.Message.TotalPrice;
        var cancellationToken = context.CancellationToken;

        try
        {
            if (await dbContext.Accounts.FirstOrDefaultAsync(_ => _.UserId == userId, cancellationToken) is not { } account)
            {
                throw new InvalidOperationException("Аккаунт пользователя не существует");
            }

            if (!account.CheckSufficientBalance(totalPrice))
            {
                throw new InvalidOperationException("Недостаточно средств на счете");
            }

            account.Withdraw(totalPrice);

            var payment = Payment.Charge(orderId, userId, totalPrice);
            dbContext.Add(payment);

            await dbContext.SaveChangesAsync(cancellationToken);

            await context.Publish(new PaymentCompleted(orderId, products), cancellationToken);
        }
        catch (Exception ex)
        {
            //await context.Publish(new PaymentFailed(orderId), _ => _.SetRoutingKey(""), cancellationToken);
            await context.Publish(new PaymentFailed(orderId, ex.Message), cancellationToken);
        }
    }
}

public class OrderCreatedConsumerDefinition : ConsumerDefinition<OrderCreatedConsumer>
{
    public OrderCreatedConsumerDefinition()
    {
        // override the default endpoint name
        //EndpointName = EndpointNames.OrdersEndpoint;

        //// limit the number of messages consumed concurrently
        //// this applies to the consumer only, not the endpoint
        //ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator, 
        IConsumerConfigurator<OrderCreatedConsumer> consumerConfigurator, 
        IRegistrationContext context)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(_ => _.Interval(5, 1000));

        // use the outbox to prevent duplicate events from being published
        //endpointConfigurator.UseInMemoryOutbox(context);
        endpointConfigurator.UseEntityFrameworkOutbox<CoreDbContext>(context);
    }
}
