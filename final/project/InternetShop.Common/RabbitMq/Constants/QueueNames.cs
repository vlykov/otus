namespace InternetShop.Common.RabbitMq.Constants;

public static class QueueNames
{
    public const string UsersQueue = "queue:users-queue";
    public const string OrdersCreatedQueue = "queue:orders-created-queue";
    public const string OrdersUnconfirmedQueue = "queue:orders-unconfirmed-queue";
    public const string OrdersConfirmedQueue = "queue:orders-confirmed-queue";
    public const string PaymentsQueue = "queue:payments-queue";
    public const string FailedPaymentsQueue = "queue:failed-payments-queue";
    public const string ProductsReservationQueue = "queue:product-reservation-queue";
    public const string FailedProductsReservationQueue = "queue:failed-product-reservation-queue";
    public const string DeliveryReservedQueue = "queue:delivery-reserved-queue";
    public const string DeliveryCompletedQueue = "queue:delivery-completed-queue";
    public const string FailedDeliveryReservationQueue = "queue:failed-delivery-reservation-queue";
}