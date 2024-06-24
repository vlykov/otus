namespace Otus.Homework.Common.Contracts.MessageBroker;

public class Events
{
    public class Orders
    {
        public record OrderCreated(int OrderId, string Product, decimal TotalPrice);
    }

    public class Billing
    {
        public record PaymentCompleted(int OrderId, string Product);
        public record PaymentFailed(int OrderId);
    }

    public class Warehouse
    {
        public record ProductReserved(int OrderId, string Product);
        public record ProductReservationFailed(int OrderId);
    }

    public class Delivery
    {
        public record DeliveryReserved(int OrderId);
        public record DeliveryReservationFailed(int OrderId);
    }
}
