using MassTransit;
using Otus.Homework.Billing.Consumers;
using Otus.Homework.Common.RabbitMq.Constants;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Billing;

namespace Otus.Homework.Billing;

public static class QueueMappings
{
    public static void MapEventsToQueues()
    {
        EndpointConvention.Map<PaymentCompleted>(new Uri(QueueNames.PaymentsQueue));
        EndpointConvention.Map<PaymentFailed>(new Uri(QueueNames.FailedPaymentsQueue));
    }

    public static void MapQueuesToReceiveEndpoints(
        this IRabbitMqBusFactoryConfigurator rabbitConfigurator,
        IBusRegistrationContext context)
    {
        rabbitConfigurator.ReceiveEndpoint(QueueNames.OrdersQueue, _ => _.ConfigureConsumer<OrderCreatedConsumer>(context));
        rabbitConfigurator.ReceiveEndpoint(QueueNames.FailedProductReservationQueue, _ => _.ConfigureConsumer<ProductReservationFailedConsumer>(context));
    }
}