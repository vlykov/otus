using MassTransit;
using InternetShop.Common.RabbitMq.Constants;
using InternetShop.Warehouse.Consumers;
using static InternetShop.Common.Contracts.MessageBroker.Events.Warehouse;

namespace InternetShop.Warehouse;

public static class QueueMappings
{
    public static void MapEventsToQueues()
    {
        EndpointConvention.Map<ProductsReserved>(new Uri(QueueNames.ProductsReservationQueue));
        EndpointConvention.Map<ProductsReservationFailed>(new Uri(QueueNames.FailedProductsReservationQueue));
    }

    public static void MapQueuesToReceiveEndpoints(
        this IRabbitMqBusFactoryConfigurator rabbitConfigurator,
        IBusRegistrationContext context)
    {
        rabbitConfigurator.ReceiveEndpoint(QueueNames.PaymentsQueue, _ => _.ConfigureConsumer<PaymentCompletedConsumer>(context));
        rabbitConfigurator.ReceiveEndpoint(QueueNames.FailedDeliveryReservationQueue, _ => _.ConfigureConsumer<DeliveryReservationFailedConsumer>(context));
    }
}