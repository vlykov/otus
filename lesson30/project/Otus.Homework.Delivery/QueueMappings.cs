using MassTransit;
using Otus.Homework.Common.RabbitMq.Constants;
using Otus.Homework.Delivery.Consumers;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Delivery;

namespace Otus.Homework.Delivery;

public static class QueueMappings
{
    public static void MapEventsToQueues()
    {
        EndpointConvention.Map<DeliveryReserved>(new Uri(QueueNames.DeliveryReservationQueue));
        EndpointConvention.Map<DeliveryReservationFailed>(new Uri(QueueNames.FailedDeliveryReservationQueue));
    }

    public static void MapQueuesToReceiveEndpoints(
        this IRabbitMqBusFactoryConfigurator rabbitConfigurator,
        IBusRegistrationContext context)
    {
        rabbitConfigurator.ReceiveEndpoint(QueueNames.ProductReservationQueue, _ => _.ConfigureConsumer<ProductReservedConsumer>(context));
    }
}