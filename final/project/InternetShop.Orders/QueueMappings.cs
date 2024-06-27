using MassTransit;
using InternetShop.Common.RabbitMq.Constants;
using InternetShop.Orders.Consumers;
using static InternetShop.Common.Contracts.MessageBroker.Events.Orders;

namespace InternetShop.Orders;

public static class QueueMappings
{
    public static void MapEventsToQueues()
    {
        EndpointConvention.Map<OrderCreated>(new Uri(QueueNames.OrdersCreatedQueue));
        EndpointConvention.Map<OrderUnconfirmed>(new Uri(QueueNames.OrdersUnconfirmedQueue));
        EndpointConvention.Map<OrderConfirmed>(new Uri(QueueNames.OrdersConfirmedQueue));        
    }

    public static void MapQueuesToReceiveEndpoints(
        this IRabbitMqBusFactoryConfigurator rabbitConfigurator,
        IBusRegistrationContext context)
    {
        rabbitConfigurator.ReceiveEndpoint(QueueNames.DeliveryReservedQueue, _ =>
        {
            _.ConfigureConsumer<DeliveryReservedConsumer>(context);
        });
        rabbitConfigurator.ReceiveEndpoint(QueueNames.DeliveryCompletedQueue, _ =>
        {
            _.ConfigureConsumer<DeliveryCompletedConsumer>(context);
        });
        rabbitConfigurator.ReceiveEndpoint(QueueNames.FailedPaymentsQueue, _ => _.ConfigureConsumer<PaymentFailedConsumer>(context));
    }
}