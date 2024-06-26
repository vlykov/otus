﻿using MassTransit;
using InternetShop.Billing.Consumers;
using InternetShop.Common.RabbitMq.Constants;
using static InternetShop.Common.Contracts.MessageBroker.Events.Billing;

namespace InternetShop.Billing;

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
        rabbitConfigurator.ReceiveEndpoint(QueueNames.OrdersCreatedQueue, _ => _.ConfigureConsumer<OrderCreatedConsumer>(context));
        rabbitConfigurator.ReceiveEndpoint(QueueNames.FailedProductsReservationQueue, _ => _.ConfigureConsumer<ProductsReservationFailedConsumer>(context));
        rabbitConfigurator.ReceiveEndpoint(QueueNames.UsersQueue, _ => _.ConfigureConsumer<UserRegisteredConsumer>(context));
    }
}