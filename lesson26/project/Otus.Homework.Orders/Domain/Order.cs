namespace Otus.Homework.Orders.Domain;

public class Order
{
    public Guid Id { get; private set; }
    public int UserId { get; private set; }
    public decimal TotalPrice { get; private set; }
    public string Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;

    public Order(int userId, decimal totalPrice)
    {
        UserId = userId;
        TotalPrice = totalPrice;
        Status = "Created";
    }

    public void SetConfirmed()
    {
        if (Status != "Created")
        {
            throw new InvalidOperationException("Некорректный статус");
        }

        Status = "Confirmed";
    }

    public void SetUnconfirmed()
    {
        if (Status != "Created")
        {
            throw new InvalidOperationException("Некорректный статус");
        }

        Status = "Unconfirmed";
    }
}
