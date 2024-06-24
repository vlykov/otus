namespace Otus.Homework.Billing.Domain;

public class Payment
{
    public int Id { get; private set; }
    public int OrderId { get; private set; }
    public string Product { get; private set; } //for testing purpose only
    public decimal TotalPrice { get; private set; }
    public string Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;

    protected Payment() { }

    public static Payment Charge(int orderId, string product, decimal totalPrice)
    {
        return new Payment
        {
            OrderId = orderId,
            Product = product,
            TotalPrice = totalPrice,
            Status = "Charged"
        };
    }

    public void ReturnMoney()
    {
        Status = "Returned";
    }
}
