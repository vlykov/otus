namespace InternetShop.Warehouse.Domain;

public class Product
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;

    protected Product() { }

    public static Product Create(string name)
    {
        return new Product
        {
            Name = name,
            Quantity = 0,
            Price = 0
        };
    }

    public bool CheckIncreasingQuantity(int amount) => amount >= 0;
    public bool CheckAvailableQuantity(int amount) => amount >= 0 && amount <= Quantity;

    public void SetQuantity(int quantity)
    {
        if (quantity < 0)
        {
            throw new InvalidOperationException("Некорректное значение для установки количества продукта");
        }

        Quantity = quantity;
    }

    public void SetPrice(decimal price)
    {
        if (price <= 0)
        {
            throw new InvalidOperationException("Некорректное значение для установки цены продукта");
        }

        Price = price;
    }

    public void IncreaseQuantity(int amount)
    {
        if (!CheckIncreasingQuantity(amount))
        {
            throw new InvalidOperationException("Некорректное значение для пополнения количества продукта");
        }

        Quantity += amount;
    }

    public void DecreaseQuantity(int amount)
    {
        if (!CheckAvailableQuantity(amount))
        {
            throw new InvalidOperationException("Некорректное значение для уменьшения количества продукта");
        }

        Quantity -= amount;
    }
}
