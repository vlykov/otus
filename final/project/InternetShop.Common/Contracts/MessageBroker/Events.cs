namespace InternetShop.Common.Contracts.MessageBroker;

public class Events
{
    public class Identity
    {
        public record UserRegistered(int UserId);
    }

    public class Orders
    {
        public record OrderCreated(int OrderId, int UserId, string Product, int Quantity, decimal TotalPrice);
        public record OrderConfirmed(int OrderId, int UserId);
        public record OrderUnconfirmed(int OrderId, int UserId);
    }

    public class Billing
    {
        public record PaymentCompleted(int OrderId, string Product, int Quantity);
        public record PaymentFailed(int OrderId, string Reason);
    }

    public class Warehouse
    {
        public record ProductReserved(int OrderId, string Product);
        public record ProductReservationFailed(int OrderId, string Reason);
    }

    public class Delivery
    {
        public record DeliveryReserved(int OrderId);
        public record DeliveryReservationFailed(int OrderId, string Reason);
        public record DeliveryCompleted(int OrderId);
    }
}
