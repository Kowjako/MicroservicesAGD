using OcelotApiGateway.Aggregator.Models;

namespace OcelotApiGateway.Aggregator.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<CatalogModel>> GetCatalog();
        Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category);
        Task<CatalogModel> GetCatalogById(string id);
    }
}
