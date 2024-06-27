using MassTransit;
using InternetShop.Common.RabbitMq.Constants;
using InternetShop.Delivery.Consumers;
using static InternetShop.Common.Contracts.MessageBroker.Events.Delivery;

namespace InternetShop.Delivery;

public static class QueueMappings
{
    public static void MapEventsToQueues()
    {
        EndpointConvention.Map<DeliveryReserved>(new Uri(QueueNames.DeliveryReservedQueue));
        EndpointConvention.Map<DeliveryCompleted>(new Uri(QueueNames.DeliveryCompletedQueue));
        EndpointConvention.Map<DeliveryReservationFailed>(new Uri(QueueNames.FailedDeliveryReservationQueue));
    }

    public static void MapQueuesToReceiveEndpoints(
        this IRabbitMqBusFactoryConfigurator rabbitConfigurator,
        IBusRegistrationContext context)
    {
        rabbitConfigurator.ReceiveEndpoint(QueueNames.ProductsReservationQueue, _ => _.ConfigureConsumer<ProductsReservedConsumer>(context));
    }
}