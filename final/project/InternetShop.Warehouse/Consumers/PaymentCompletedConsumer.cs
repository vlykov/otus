using MassTransit;
using InternetShop.Warehouse.Domain;
using InternetShop.Warehouse.Infrastructure.Persistence;
using static InternetShop.Common.Contracts.MessageBroker.Events.Billing;
using static InternetShop.Common.Contracts.MessageBroker.Events.Warehouse;
using Microsoft.EntityFrameworkCore;

namespace InternetShop.Warehouse.Consumers;

public class PaymentCompletedConsumer(CoreDbContext dbContext) : IConsumer<PaymentCompleted>
{
    public async Task Consume(ConsumeContext<PaymentCompleted> context)
    {
        var orderId = context.Message.OrderId;
        var productToReserve = context.Message.Product;
        var quantity = context.Message.Quantity;
        var cancellationToken = context.CancellationToken;

        try
        {
            if (await dbContext.Products.FirstOrDefaultAsync(_ => _.Name == productToReserve, cancellationToken) is not { } product)
            {
                throw new InvalidOperationException("Товар отсутствует на складе");
            }
            if (!product.CheckAvailableQuantity(quantity))
            {
                throw new InvalidOperationException("Недостаточное количество товара на складе");
            }

            product.DecreaseQuantity(quantity);
            var reservation = Reservation.Reserve(orderId, product.Id, productToReserve, quantity);

            dbContext.Add(reservation);
            await dbContext.SaveChangesAsync(cancellationToken);

            await context.Publish(new ProductReserved(orderId, productToReserve), cancellationToken);
        }
        catch (Exception ex)
        {
            await context.Publish(new ProductReservationFailed(orderId, ex.Message), cancellationToken);
        }
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

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<PaymentCompletedConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(_ => _.Interval(5, 1000));

        // use the outbox to prevent duplicate events from being published
        //endpointConfigurator.UseInMemoryOutbox(context);
        endpointConfigurator.UseEntityFrameworkOutbox<CoreDbContext>(context);
    }
}
