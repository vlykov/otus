using MassTransit;
using InternetShop.Common.RabbitMq.Constants;
using InternetShop.Orders.Consumers;
using static InternetShop.Common.Contracts.MessageBroker.Events.Orders;

namespace InternetShop.Orders;

public static class QueueMappings
{
    public static void MapEventsToQueues()
    {
        EndpointConvention.Map<OrderCreated>(new Uri(QueueNames.OrdersQueue));
    }

    public static void MapQueuesToReceiveEndpoints(
        this IRabbitMqBusFactoryConfigurator rabbitConfigurator,
        IBusRegistrationContext context)
    {
        rabbitConfigurator.ReceiveEndpoint(QueueNames.DeliveryQueue, _ => _.ConfigureConsumer<DeliveryReservedConsumer>(context));
        rabbitConfigurator.ReceiveEndpoint(QueueNames.DeliveryQueue, _ => _.ConfigureConsumer<DeliveryCompletedConsumer>(context));
        rabbitConfigurator.ReceiveEndpoint(QueueNames.FailedPaymentsQueue, _ => _.ConfigureConsumer<PaymentFailedConsumer>(context));
    }
}