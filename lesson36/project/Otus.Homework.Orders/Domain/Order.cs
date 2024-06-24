namespace Otus.Homework.Orders.Domain;

public class Order(string product, decimal totalPrice)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Product { get; private set; } = product;
    public decimal TotalPrice { get; private set; } = totalPrice;
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;
}
