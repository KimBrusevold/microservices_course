using System.Text.Json;
using Basket.API.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Repositories;

internal sealed class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _cache;

    public BasketRepository(IDistributedCache cache)
    { 
        
        _cache = cache ?? throw new ArgumentNullException("Must have Cache reference");
    }

    public async Task<ShoppingCart?> GetBasket(string userName)
    {
        var basket = await _cache.GetStringAsync(userName);

        if(string.IsNullOrWhiteSpace(basket))
            return null;

        return JsonSerializer.Deserialize<ShoppingCart>(basket);
    }

    public async Task<ShoppingCart?> UpdateBasket(ShoppingCart basket)
    {
        await _cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket));   
        return await GetBasket(basket.UserName);        
    }

    public async Task DeleteBasket(string username)
    {
        await _cache.RemoveAsync(username);
    }
}