namespace Payments.Domain.Orders.StateMachine.Models
{
    public class AddProductToOrderData
    {
        public AddProductToOrderData(string name, int count, decimal price)
        {
            Name = name;
            Count = count;
            Price = price;
        }

        public string Name { get; }
        public int Count { get; }
        public decimal Price { get; }
    }
}