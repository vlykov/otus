namespace InternetShop.Orders.Domain;

public class OrderPosition(int productId, string name, int quantity, decimal price)
{
    public int Id { get; private set; }
    public int ProductId { get; private set; } = productId;
    public string Name { get; private set; } = name;
    public int Quantity { get; private set; } = quantity;
    public decimal Price { get; private set; } = price;

    public Order? Order { get; private set; }
}
