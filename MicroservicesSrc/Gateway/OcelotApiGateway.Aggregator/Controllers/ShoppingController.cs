using Microsoft.AspNetCore.Mvc;
using OcelotApiGateway.Aggregator.Models.Root;
using OcelotApiGateway.Aggregator.Services;

namespace OcelotApiGateway.Aggregator.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public ShoppingController(ICatalogService catalogSvc, IBasketService basketSvc, IOrderService orderSvc)
        {
            _catalogService = catalogSvc;
            _basketService = basketSvc;
            _orderService = orderSvc;
        }

        [HttpGet("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShoppingModel))]
        public async Task<ActionResult<ShoppingModel>> GetShopping([FromRoute] string username)
        {
            var basket = await _basketService.GetBasket(username);
            foreach (var item in basket.Items)
            {
                var product = await _catalogService.GetCatalogById(item.ProductId);
                item.ProductName = product.Name;
                item.Category = product.Category;
                item.Summary = product.Summary;
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;
            }

            var orders = await _orderService.GetOrdersByUsername(username);

            var shoppingModel = new ShoppingModel
            {
                Username = username,
                BasketWithProducts = basket,
                Orders = orders
            };

            return Ok(shoppingModel);
        }
    }
}
