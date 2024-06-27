namespace InternetShop.Common.Contracts.MessageBroker;

public class Models
{
    public class Orders
    {
        public class OrderPosition(int id, string name, int quantity, decimal price)
        {
            public int Id { get; private set; } = id;
            public string Name { get; private set; } = name;
            public int Quantity { get; private set; } = quantity;
            public decimal Price { get; private set; } = price;
        }
    }
}
