using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data;
internal sealed class CatalogContext : ICatalogContext
{
    public CatalogContext(IConfiguration configuration)
    {
        var section = configuration.GetSection("DatabaseSettings");

        var client = new MongoClient(section.GetValue<string>("ConnectionString"));
        var database = client.GetDatabase(section.GetValue<string>("DatabaseName"));
        
        Products = database.GetCollection<Product>(section.GetValue<string>("CollectionName"));
        CatalogContextSeed.SeedData(Products);
    }

    public IMongoCollection<Product> Products { get; init; }
}
