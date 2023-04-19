using MediatR;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommand : IRequest<int>
    {
        public OrderDTO Order { get; set; }
    }
}
