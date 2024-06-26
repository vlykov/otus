using MassTransit;
using InternetShop.Common.RabbitMq.Constants;
using InternetShop.Warehouse.Consumers;
using static InternetShop.Common.Contracts.MessageBroker.Events.Warehouse;

namespace InternetShop.Warehouse;

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
        rabbitConfigurator.ReceiveEndpoint(QueueNames.FailedDeliveryQueue, _ => _.ConfigureConsumer<DeliveryReservationFailedConsumer>(context));
    }
}