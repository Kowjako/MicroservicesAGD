namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public string Username { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new();

        public decimal TotalPrice
        {
            get
            {
                return Items.Aggregate(0m, (sum, item) => sum += item.Price * item.Quantity);
            }
        }
    }
}
