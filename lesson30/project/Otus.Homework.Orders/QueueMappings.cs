using MassTransit;
using Otus.Homework.Common.RabbitMq.Constants;
using Otus.Homework.Orders.Consumers;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Orders;

namespace Otus.Homework.Orders;

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
        rabbitConfigurator.ReceiveEndpoint(QueueNames.DeliveryReservationQueue, _ => _.ConfigureConsumer<DeliveryReservedConsumer>(context));
        rabbitConfigurator.ReceiveEndpoint(QueueNames.FailedPaymentsQueue, _ => _.ConfigureConsumer<PaymentFailedConsumer>(context));
    }
}