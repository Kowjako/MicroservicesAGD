using Basket.API.Entities;
using Basket.API.gRPC;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepo;
        private readonly DiscountGrpcProxy _discountGrpc;

        public BasketController(IBasketRepository basketRepo, DiscountGrpcProxy discountGrpc)
            => (_basketRepo, _discountGrpc) = (basketRepo, discountGrpc);

        [HttpGet("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShoppingCart))]
        public async Task<ActionResult<ShoppingCart>> GetBasket([FromRoute] string username)
        {
            var basket = await _basketRepo.GetBasket(username);
            return Ok(basket ?? new ShoppingCart() { Username = username });
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShoppingCart))]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart cart)
        {
            // Call Discount gRPC via proxy and recieve discount for item
            foreach(var item in cart.Items)
            {
                var coupon = await _discountGrpc.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            return Ok(await _basketRepo.UpdateBasket(cart));
        }

        [HttpDelete("{username}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteBasket([FromRoute]string username)
        {
            await _basketRepo.DeleteBasket(username);
            return NoContent();
        }
    }
}
