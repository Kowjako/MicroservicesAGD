using Discount.API.Entities;
using Discount.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _repo;

        public DiscountController(IDiscountRepository repo) => _repo = repo;

        [HttpGet("{productName}", Name = "GetDiscountByProdName")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Coupon))]
        public async Task<ActionResult<Coupon>> GetDiscount([FromRoute] string productName)
        {
            return Ok(await _repo.GetDiscount(productName));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Coupon))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon)
        {
            if (await _repo.CreateDiscount(coupon))
            {
                return CreatedAtRoute("GetDiscountByProdName", new { productName = coupon.ProductName }, coupon);
            }
            return BadRequest("Can't create coupon");
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateDiscount([FromBody] Coupon coupon)
        {
            if (await _repo.UpdateDiscount(coupon))
            {
                return NoContent();
            }
            return BadRequest("Can't update coupon");
        }

        [HttpDelete("{productName}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Coupon>> DeleteDiscount([FromRoute] string productName)
        {
            if (await _repo.DeleteDiscount(productName))
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
