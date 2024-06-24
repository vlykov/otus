namespace Otus.Homework.Warehouse.Domain;

public class Reservation
{
    public int Id { get; private set; }
    public int OrderId { get; private set; }
    public string Product { get; private set; }
    public string Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;

    protected Reservation() { }

    public static Reservation Reserve(int orderId, string product)
    {
        return new Reservation
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
