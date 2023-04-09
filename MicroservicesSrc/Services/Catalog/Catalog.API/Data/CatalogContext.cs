using Catalog.API.Entities;
using Catalog.API.Extensions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> Products { get; }

        public CatalogContext(IOptions<MongoSettings> config)
        {
            var client = new MongoClient(config.Value.ConnectionString);
            var db = client.GetDatabase(config.Value.DatabaseName);

            Products = db.GetCollection<Product>(config.Value.CollectionName);
            CatalogContextSeed.SeedData(Products);
        }

    }
}
