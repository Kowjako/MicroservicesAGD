using OcelotApiGateway.Aggregator.Models;

namespace OcelotApiGateway.Aggregator.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUsername(string username)
        {
            return await _httpClient.GetFromJsonAsync<List<OrderResponseModel>>($"/api/v1/Order/{username}");
        }
    }
}
