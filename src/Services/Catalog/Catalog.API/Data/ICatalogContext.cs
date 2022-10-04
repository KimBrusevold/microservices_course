using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data;
internal interface ICatalogContext
{
    IMongoCollection<Product> Products { get; }
}