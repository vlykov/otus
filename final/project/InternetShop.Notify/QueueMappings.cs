using MassTransit;
using InternetShop.Common.RabbitMq.Constants;
using InternetShop.Notify.Consumers;

namespace InternetShop.Notify;

public static class QueueMappings
{
    public static void MapEventsToQueues()
    {
    }

    public static void MapQueuesToReceiveEndpoints(
        this IRabbitMqBusFactoryConfigurator rabbitConfigurator,
        IBusRegistrationContext context)
    {
        rabbitConfigurator.ReceiveEndpoint(QueueNames.OrdersQueue, _ => _.ConfigureConsumer<OrderConfirmedConsumer>(context));
        rabbitConfigurator.ReceiveEndpoint(QueueNames.OrdersQueue, _ => _.ConfigureConsumer<OrderUnconfirmedConsumer>(context));
    }
}