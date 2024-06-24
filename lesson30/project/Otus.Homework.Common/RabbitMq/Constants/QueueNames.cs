namespace Otus.Homework.Common.RabbitMq.Constants;

public static class QueueNames
{
    public const string OrdersQueue = "queue:orders-queue";
    public const string PaymentsQueue = "queue:payments-queue";
    public const string FailedPaymentsQueue = "queue:failed-payments-queue";
    public const string ProductReservationQueue = "queue:product-reservation-queue";
    public const string FailedProductReservationQueue = "queue:failed-product-reservation-queue";
    public const string DeliveryReservationQueue = "queue:delivery-reservation-queue";
    public const string FailedDeliveryReservationQueue = "queue:failed-delivery-reservation-queue";
}