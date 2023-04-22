using OcelotApiGateway.Aggregator.Models;

namespace OcelotApiGateway.Aggregator.Services
{
    public interface IBasketService
    {
        Task<BasketModel> GetBasket(string username);
    }
}
