using MassTransit;
using InternetShop.Common.RabbitMq.Constants;
using InternetShop.Delivery.Consumers;
using static InternetShop.Common.Contracts.MessageBroker.Events.Delivery;

namespace InternetShop.Delivery;

public static class QueueMappings
{
    public static void MapEventsToQueues()
    {
        EndpointConvention.Map<DeliveryReserved>(new Uri(QueueNames.DeliveryQueue));
        EndpointConvention.Map<DeliveryReservationFailed>(new Uri(QueueNames.FailedDeliveryQueue));
        EndpointConvention.Map<DeliveryCompleted>(new Uri(QueueNames.DeliveryQueue));
    }

    public static void MapQueuesToReceiveEndpoints(
        this IRabbitMqBusFactoryConfigurator rabbitConfigurator,
        IBusRegistrationContext context)
    {
        rabbitConfigurator.ReceiveEndpoint(QueueNames.ProductReservationQueue, _ => _.ConfigureConsumer<ProductReservedConsumer>(context));
    }
}