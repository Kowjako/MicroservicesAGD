using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ILogger<CatalogController> _logger;
        private readonly IProductRepository _productRepo;

        public CatalogController(IProductRepository productRepo, ILogger<CatalogController> logger)
            => (_productRepo, _logger) = (productRepo, logger);

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepo.GetProducts();
            return Ok(products);
        }

        // Name is used there for CreatedAtRoute status code
        [HttpGet("{id:length(24)}", Name = "ShowProduct")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        public async Task<ActionResult<Product>> GetProductById([FromRoute] string id)
        {
            var product = await _productRepo.GetProductById(id);
            if (product == null)
            {
                _logger.LogError($"Product with id: {id} not found");
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("[action]/{category}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory([FromRoute] string category)
        {
            var products = await _productRepo.GetProductByCategory(category);
            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Product))]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _productRepo.CreateProduct(product);
            // produce response with created route & created object
            return CreatedAtRoute("ShowProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateProduct([FromBody] Product product)
        {
            if (!await _productRepo.UpdateProduct(product))
            {
                return BadRequest("Can't update specified product");
            }
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteProduct([FromRoute] string id)
        {
            if (!await _productRepo.DeleteProduct(id))
            {
                return BadRequest($"Can't delete product with id = {id}");
            }
            return NoContent();
        }
    }
}
