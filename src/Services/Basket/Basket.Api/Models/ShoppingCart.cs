namespace Basket.API.Models;
public record struct ShoppingCart
{
    public string UserName { get; set; }
    public List<ShoppingCartItem> Items { get; set; } = new();

    public ShoppingCart(string userName)
    {
        UserName = userName;
    }

    public decimal TotalPrice
    {
        get
        {
            decimal totalPrice = 0;
            for(var i = 0 ; i < Items.Count; i++)
            {
                totalPrice += Items[i].Price * Items[i].Quantity;
            }

            return totalPrice;
        }
    }
}

public record struct ShoppingCartItem(int Quantity, string Color, decimal Price, string ProductId, string ProductName);