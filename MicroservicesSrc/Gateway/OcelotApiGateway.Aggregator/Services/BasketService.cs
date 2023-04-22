using OcelotApiGateway.Aggregator.Models;

namespace OcelotApiGateway.Aggregator.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;

        public BasketService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BasketModel> GetBasket(string username)
        {
            return await _httpClient.GetFromJsonAsync<BasketModel>($"/api/v1/Basket/{username}");
        }
    }
}
