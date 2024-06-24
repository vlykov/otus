namespace Otus.Homework.Orders.Domain;

public class Order(string product, decimal totalPrice)
{
    public int Id { get; private set; }    
    public string Product { get; private set; } = product;
    public decimal TotalPrice { get; private set; } = totalPrice;
    public string Status { get; private set; } = "Created";
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;

    public void SetConfirmed()
    {
        if (Status != "Created")
        {
            throw new InvalidOperationException("Некорректный статус");
        }

        Status = "Confirmed";
    }

    public void SetDeclined()
    {
        if (Status != "Created")
        {
            throw new InvalidOperationException("Некорректный статус");
        }

        Status = "Declined";
    }
}
