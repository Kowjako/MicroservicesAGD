using OcelotApiGateway.Aggregator.Models;

namespace OcelotApiGateway.Aggregator.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalog()
        {
            return await _httpClient.GetFromJsonAsync<List<CatalogModel>>("/api/v1/Catalog");
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category)
        {
            return await _httpClient.GetFromJsonAsync<List<CatalogModel>>($"/api/v1/Catalog/GetProductByCategory/{category}");
        }

        public async Task<CatalogModel> GetCatalogById(string id)
        {
            return await _httpClient.GetFromJsonAsync<CatalogModel>($"/api/v1/Catalog/{id}");
        }
    }
}
