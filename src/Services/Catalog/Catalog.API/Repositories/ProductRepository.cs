using Catalog.Api.Entities;
using Catalog.API.Data;
using Microsoft.Toolkit.Diagnostics;
using MongoDB.Driver;

namespace Catalog.API.Repositories;
internal sealed class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _context;

    public ProductRepository(ICatalogContext context)
    {
        Guard.IsNotNull(context, nameof(context));
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        var result = await _context
            .Products
            .FindAsync(FilterDefinition<Product>.Empty);
            
        return await result.ToListAsync();
    }

    public async Task<Product> GetProduct(string id)
    {
        var result = await _context.Products
            .FindAsync(Builders<Product>.Filter.Eq(p => p.Id, id));
        
        return await result.SingleAsync();
    }
    public async Task<IEnumerable<Product>> GetProductsByCategory(string categoryName)
    {
        var result = await _context.Products
            .FindAsync(Builders<Product>.Filter.Eq(p => p.Category, categoryName));
        
        return await result.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByName(string name)
    {
        var result = await _context.Products
            .FindAsync(Builders<Product>.Filter.Eq(p => p.Name, name));
        
        return await result.ToListAsync();
    }
    public async Task CreateProduct(Product product)
    {
        await _context.Products.InsertOneAsync(product);
    }

    public async Task<bool> DeleteProduct(string id)
    {
        var deleteResult = await _context.Products.DeleteOneAsync(filter: p => p.Id == id);
        return deleteResult.IsAcknowledged 
            && deleteResult.DeletedCount > 0;
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        var updateresult = await _context.Products
            .ReplaceOneAsync(filter: g=> g.Id == product.Id, replacement: product);
        
        return updateresult.IsAcknowledged 
            && updateresult.ModifiedCount > 0;
    }
}
