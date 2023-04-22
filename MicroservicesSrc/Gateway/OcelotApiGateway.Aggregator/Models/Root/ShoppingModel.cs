namespace OcelotApiGateway.Aggregator.Models.Root
{
    public class ShoppingModel
    {
        public string Username { get; set; }
        public BasketModel BasketWithProducts { get; set; }
        public IEnumerable<OrderResponseModel> Orders { get; set; }
    }
}
