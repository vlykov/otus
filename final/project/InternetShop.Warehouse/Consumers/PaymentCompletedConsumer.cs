using MassTransit;
using InternetShop.Warehouse.Domain;
using InternetShop.Warehouse.Infrastructure.Persistence;
using static InternetShop.Common.Contracts.MessageBroker.Events.Billing;
using static InternetShop.Common.Contracts.MessageBroker.Events.Warehouse;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace InternetShop.Warehouse.Consumers;

public class PaymentCompletedConsumer(CoreDbContext dbContext) : IConsumer<PaymentCompleted>
{
    public async Task Consume(ConsumeContext<PaymentCompleted> context)
    {
        var orderId = context.Message.OrderId;
        var productsToReserve = context.Message.Products;
        var cancellationToken = context.CancellationToken;

        try
        {
            var ids = productsToReserve.Select(_ => _.Id).ToList();
            var dbProducts = await dbContext.Products.Where(_ => ids.Contains(_.Id)).ToListAsync(cancellationToken);

            if (ids.Count != dbProducts.Count)
            {
                throw new InvalidOperationException("Один из товар отсутствует в номенклатуре склада");
            }

            var hasDuplicates = productsToReserve.GroupBy(product => product.Id)
              .Any(group => group.Count() > 1);

            if (hasDuplicates)
            {
                throw new InvalidOperationException("Присутствуют дубликаты товара в запросе на резерв. Запрос должен содержать уникальный товар в нужном количестве");
            }

            foreach (var product in dbProducts)
            {
                var quantity = productsToReserve.First(_ => _.Id == product.Id).Quantity;
                if (!product.CheckAvailableQuantity(quantity))
                {
                    throw new InvalidOperationException($"Недостаточное количество товара '{product.Name}' на складе. Id товара '{product.Id}'");
                }

                product.DecreaseQuantity(quantity);

                var reservation = Reservation.Reserve(orderId, product.Id, quantity);

                dbContext.Add(reservation);
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            await context.Publish(new ProductsReserved(orderId, productsToReserve), cancellationToken);
        }
        catch (Exception ex)
        {
            await context.Publish(new ProductsReservationFailed(orderId, ex.Message), cancellationToken);
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
