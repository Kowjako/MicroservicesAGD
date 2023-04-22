using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using Ordering.Domain.Entities;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public OrdersController(IMediator mediatR) => _mediatR = mediatR;

        [HttpGet("{username}", Name = "GetOrder")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderDTO>))]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrdersByUserName([FromRoute] string username)
        {
            var query = new GetOrdersListQuery()
            {
                UserName = username
            };

            var orders = await _mediatR.Send(query);
            return Ok(orders);
        }

        // Only for test - checkout will be triggered via RabbitMQ event bus
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] OrderDTO order)
        {
            var cmd = new CheckoutOrderCommand()
            {
                Order = order
            };

            var result = await _mediatR.Send(cmd);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> UpdateOrder([FromRoute] int id, [FromBody] UpdateOrderDTO order)
        {
            var cmd = new UpdateOrderCommand()
            {
                Id = id,
                Order = order
            };

            await _mediatR.Send(cmd);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> DeleteOrder([FromRoute] int id)
        {
            var cmd = new DeleteOrderCommand()
            {
                OrderId = id
            };

            await _mediatR.Send(cmd);
            return NoContent();
        }
    }
}
