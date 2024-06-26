using MassTransit;
using InternetShop.Common.RabbitMq.Constants;
using static InternetShop.Common.Contracts.MessageBroker.Events.Identity;

namespace InternetShop.Identity;

public static class QueueMappings
{
    public static void MapEventsToQueues()
    {
        EndpointConvention.Map<UserRegistered>(new Uri(QueueNames.UsersQueue));
    }
}