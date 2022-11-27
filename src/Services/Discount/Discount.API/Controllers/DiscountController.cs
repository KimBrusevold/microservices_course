using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Discount.API.Entities;
using System.Net;

namespace Discount.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DiscountController : ControllerBase
{

    private readonly ILogger<DiscountController> _logger;
    private readonly IDiscountRepository _repository; 
    public DiscountController(IDiscountRepository repository, ILogger<DiscountController> logger)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet("{productName}", Name = nameof(GetDiscount))]
    [ProducesResponseType(typeof(Coupon), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<Coupon>> GetDiscount(string productName)
    {
        var coupon = await _repository.GetDiscount(productName);
        return Ok(coupon);
    }

    [HttpPost(Name = nameof(CreateDiscount))]
    [ProducesResponseType(typeof(Coupon), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon)
    {
        await _repository.CreateDiscount(coupon);
        return CreatedAtRoute("GetDiscount", new { coupon.ProductName}, coupon);
    }

    
    [HttpPut(Name = nameof(UpdateDiscount))]
    [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> UpdateDiscount([FromBody] Coupon coupon)
    {
        return Ok(await _repository.UpdateDiscount(coupon));
    }

    [HttpDelete("{productName}", Name = nameof(DeleteDiscount))]
    [ProducesResponseType(typeof(Coupon), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> DeleteDiscount(string productName)
    {
        return Ok(await _repository.DeleteDiscount(productName));
    }
}
