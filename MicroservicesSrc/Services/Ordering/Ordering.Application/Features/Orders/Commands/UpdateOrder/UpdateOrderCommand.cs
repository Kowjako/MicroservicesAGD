using MediatR;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public UpdateOrderDTO Order { get; set; }
    }
}
