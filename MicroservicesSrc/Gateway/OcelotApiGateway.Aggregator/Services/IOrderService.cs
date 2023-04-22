using OcelotApiGateway.Aggregator.Models;

namespace OcelotApiGateway.Aggregator.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseModel>> GetOrdersByUsername(string username);
    }
}
