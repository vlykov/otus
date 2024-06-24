namespace Otus.Homework.Delivery.Domain;

public class Courier
{
    public int Id { get; private set; }
    public int OrderId { get; private set; }
    public string Product { get; private set; }
    public string Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;

    protected Courier() { }

    public static Courier Reserve(int orderId, string product)
    {
        return new Courier
        {
            OrderId = orderId,
            Product = product,
            Status = "Reserved"
        };
    }

    public void Decline()
    {
        if (Status != "Reserved")
        {
            throw new InvalidOperationException("Некорректный статус");
        }

        Status = "Declined";
    }
}
