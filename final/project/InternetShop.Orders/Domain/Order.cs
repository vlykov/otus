namespace InternetShop.Orders.Domain;

public class Order(int userId, string product, int quantity, decimal totalPrice)
{
    public int Id { get; private set; }
    public int UserId { get; private set; } = userId;
    public string Product { get; private set; } = product;
    public int Quantity { get; private set; } = quantity;
    public decimal TotalPrice { get; private set; } = totalPrice;
    public string Status { get; private set; } = "Created";
    public string? Reason { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;

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
