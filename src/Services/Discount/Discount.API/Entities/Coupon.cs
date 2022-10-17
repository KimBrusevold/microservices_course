using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.API.Entities;
public record struct Coupon
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public int Amount { get; set; }

    public Coupon(int id, string productName, string description, int amount)
    {
        Id = id;
        ProductName = productName;
        Description = description;
        Amount = amount;
    }

    public static Coupon Default()
        => new Coupon
            {
                ProductName = "No Discount",
                Amount = 0,
                Description = "No Discount Description"
            };
    
}