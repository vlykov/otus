namespace Otus.Homework.Common.Contracts.MessageBroker;

public class Events
{
    public class Identity
    {
        public record UserRegistered(int UserId);
    }

    public class Orders
    {
        public record OrderCreated(Guid OrderId, int UserId, decimal TotalPrice);
        public record OrderConfirmed(Guid OrderId, int UserId);
        public record OrderUnconfirmed(Guid OrderId, int UserId);
    }

    public class Billing
    {
        public record OrderPaid(Guid OrderId);
        public record OrderUnpaid(Guid OrderId);
    }
}
