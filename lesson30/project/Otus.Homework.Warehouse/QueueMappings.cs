using MassTransit;
using Otus.Homework.Common.RabbitMq.Constants;
using Otus.Homework.Warehouse.Consumers;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Warehouse;

namespace Otus.Homework.Warehouse;

public static class QueueMappings
{
    public static void MapEventsToQueues()
    {
        EndpointConvention.Map<ProductReserved>(new Uri(QueueNames.ProductReservationQueue));
        EndpointConvention.Map<ProductReservationFailed>(new Uri(QueueNames.FailedProductReservationQueue));
    }

    public static void MapQueuesToReceiveEndpoints(
        this IRabbitMqBusFactoryConfigurator rabbitConfigurator,
        IBusRegistrationContext context)
    {
        rabbitConfigurator.ReceiveEndpoint(QueueNames.PaymentsQueue, _ => _.ConfigureConsumer<PaymentCompletedConsumer>(context));
        rabbitConfigurator.ReceiveEndpoint(QueueNames.FailedDeliveryReservationQueue, _ => _.ConfigureConsumer<DeliveryReservationFailedConsumer>(context));
    }
}