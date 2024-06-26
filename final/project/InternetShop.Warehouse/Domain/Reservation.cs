namespace InternetShop.Warehouse.Domain;

public class Reservation
{
    public int Id { get; private set; }
    public int OrderId { get; private set; }
    public int ProductId { get; private set; }
    public string Product { get; private set; }
    public int Quantity { get; internal set; }
    public string Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;

    protected Reservation() { }

    public static Reservation Reserve(int orderId, int productId, string product, int quantity)
    {
        return new Reservation
        {
            OrderId = orderId,
            Product = product,
            ProductId = productId,
            Quantity = quantity,
            Status = "Reserved"
        };
    }

    public void Decline()
    {
        if (Status != "Reserved")
        {
            throw new InvalidOperationException($"Некорректный статус резервирования товара {Status}");
        }

        Status = "Declined";
    }

    public void HandoverToDelivery()
    {
        if (Status != "Reserved")
        {
            throw new InvalidOperationException($"Некорректный статус резервирования товара {Status}");
        }

        Status = "Handovered";
    }    
}
