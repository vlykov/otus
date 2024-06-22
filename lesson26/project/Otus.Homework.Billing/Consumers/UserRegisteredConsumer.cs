using MassTransit;
using Microsoft.EntityFrameworkCore;
using Otus.Homework.Billing.Domain;
using Otus.Homework.Billing.Infrastructure.Persistence;
using Otus.Homework.Common.RabbitMq.Constants;
using static Otus.Homework.Common.Contracts.MessageBroker.Events.Identity;

namespace Otus.Homework.Billing.Consumers;

public class UserRegisteredConsumer(AccountContext dbContext) : IConsumer<UserRegistered>
{
    public async Task Consume(ConsumeContext<UserRegistered> context)
    {
        var userId = context.Message.UserId;
        var cancellationToken = context.CancellationToken;

        if (!await dbContext.Accounts.AsNoTracking().AnyAsync(_ => _.UserId == userId, cancellationToken))
        {
            dbContext.Accounts.Add(new Account(userId));
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

public class UserRegisteredConsumerDefinition : ConsumerDefinition<UserRegisteredConsumer>
{
    public UserRegisteredConsumerDefinition()
    {
        // override the default endpoint name
        EndpointName = EndpointNames.UsersEndpoint;

        //// limit the number of messages consumed concurrently
        //// this applies to the consumer only, not the endpoint
        //ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<UserRegisteredConsumer> consumerConfigurator)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(_ => _.Interval(5, 1000));

        // use the outbox to prevent duplicate events from being published
        endpointConfigurator.UseInMemoryOutbox();
    }
}
