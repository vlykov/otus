namespace InternetShop.Orders.Domain;

public class Order
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public decimal TotalPrice { get; private set; }
    public string Status { get; private set; } = "Created";
    public string? Reason { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;

    public ICollection<OrderPosition> Positions { get; private set; }

    protected Order() { }

    public Order(int userId, ICollection<OrderPosition> orderPositions)
    {
        UserId = userId;
        Positions = orderPositions.ToList();
        TotalPrice = orderPositions.Sum(_ => _.Price * _.Quantity);
    }

    public void SetConfirmed()
    {
        if (Status != "Created")
        {
            throw new InvalidOperationException($"Некорректный статус заказа {Status}");
        }

        Status = "Confirmed";
    }

    public void SetCompleted()
    {
        if (Status != "Confirmed")
        {
            throw new InvalidOperationException($"Некорректный статус заказа {Status}");
        }

        Status = "Completed";
    }

    public void SetDeclined(string? reason)
    {
        if (Status != "Created")
        {
            throw new InvalidOperationException($"Некорректный статус заказа {Status}");
        }

        Status = "Declined";
        Reason = reason;
    }
}
