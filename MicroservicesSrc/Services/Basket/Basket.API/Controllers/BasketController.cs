using AutoMapper;
using Basket.API.Entities;
using Basket.API.gRPC;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepo;
        private readonly DiscountGrpcProxy _discountGrpc;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _rabbitQueue;

        public BasketController(IBasketRepository basketRepo,
                                DiscountGrpcProxy discountGrpc,
                                IMapper mapper,
                                IPublishEndpoint rabbitQueue)
            => (_basketRepo, _discountGrpc, _mapper, _rabbitQueue) = (basketRepo, discountGrpc, mapper, rabbitQueue);

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
            foreach (var item in cart.Items)
            {
                var coupon = await _discountGrpc.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            return Ok(await _basketRepo.UpdateBasket(cart));
        }

        [HttpDelete("{username}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteBasket([FromRoute] string username)
        {
            await _basketRepo.DeleteBasket(username);
            return NoContent();
        }

        [HttpPost("checkout")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CheckoutBasket([FromBody] BasketCheckout basket)
        {
            var userBasket = await _basketRepo.GetBasket(basket.UserName);
            if (userBasket is null)
            {
                return BadRequest("Can't find specified basket");
            }

            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basket);
            await _rabbitQueue.Publish(eventMessage);

            await _basketRepo.DeleteBasket(basket.UserName);
            return Accepted();
        }
    }
}
