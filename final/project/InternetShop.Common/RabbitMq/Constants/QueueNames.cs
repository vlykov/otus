namespace InternetShop.Common.RabbitMq.Constants;

public static class QueueNames
{
    public const string UsersQueue = "queue:users-queue";
    public const string OrdersQueue = "queue:orders-queue";
    public const string PaymentsQueue = "queue:payments-queue";
    public const string FailedPaymentsQueue = "queue:failed-payments-queue";
    public const string ProductReservationQueue = "queue:product-reservation-queue";
    public const string FailedProductReservationQueue = "queue:failed-product-reservation-queue";
    public const string DeliveryQueue = "queue:delivery-queue";
    public const string FailedDeliveryQueue = "queue:failed-reservation-queue";
}