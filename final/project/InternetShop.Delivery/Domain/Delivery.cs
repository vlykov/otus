namespace InternetShop.Delivery.Domain;

public class Delivery
{
    public int Id { get; private set; }
    public int OrderId { get; private set; }
    public int CourierId { get; private set; }
    public string Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;

    protected Delivery() { }

    public static Delivery Reserve(int orderId)
    {
        return new Delivery
        {
            OrderId = orderId,
            CourierId = 1,
            Status = "Reserved"
        };
    }

    public void Decline()
    {
        if (Status != "Reserved")
        {
            throw new InvalidOperationException($"Некорректный статус доставки {Status}");
        }

        Status = "Declined";
    }

    //public void Delivery()
    //{
    //    if (Status != "Reserved")
    //    {
    //        throw new InvalidOperationException($"Некорректный статус доставки {Status}");
    //    }

    //    Status = "InDelivery";
    //}

    //public void Deliveried()
    //{
    //    if (Status != "InDelivery")
    //    {
    //        throw new InvalidOperationException($"Некорректный статус доставки {Status}");
    //    }

    //    Status = "Deliveried";
    //}

    public void Deliveried()
    {
        if (Status != "Reserved")
        {
            throw new InvalidOperationException($"Некорректный статус доставки {Status}");
        }

        Status = "Deliveried";
    }
}
