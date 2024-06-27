using static InternetShop.Common.Contracts.MessageBroker.Models.Orders;

namespace InternetShop.Common.Contracts.MessageBroker;

public class Events
{
    public class Identity
    {
        public record UserRegistered(int UserId);
    }

    public class Orders
    {
        public record OrderCreated(int OrderId, int UserId, ICollection<OrderPosition> Products, decimal TotalPrice);
        public record OrderConfirmed(int OrderId, int UserId);
        public record OrderUnconfirmed(int OrderId, int UserId, string Reason);
    }

    public class Billing
    {
        public record PaymentCompleted(int OrderId, ICollection<OrderPosition> Products);
        public record PaymentFailed(int OrderId, string Reason);
    }

    public class Warehouse
    {
        public record ProductsReserved(int OrderId, ICollection<OrderPosition> Products);
        public record ProductsReservationFailed(int OrderId, string Reason);
    }

    public class Delivery
    {
        public record DeliveryReserved(int OrderId);
        public record DeliveryReservationFailed(int OrderId, string Reason);
        public record DeliveryCompleted(int OrderId);
    }
}
