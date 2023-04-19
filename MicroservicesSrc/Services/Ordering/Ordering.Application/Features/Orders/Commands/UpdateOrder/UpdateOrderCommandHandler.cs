using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
    {
        private readonly IOrderRepository _repo;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public UpdateOrderCommandHandler(IOrderRepository repo, ILogger<CheckoutOrderCommandHandler> logger)
            => (_repo, _logger) = (repo, logger);

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var originalOrder = await _repo.GetByIdAsync(request.Order.Id);
            if(originalOrder is null)
            {
                _logger.LogError("Order not exist on database");
                throw new NotFoundException(nameof(Order), request.Order.Id);
            }

            originalOrder = request.Order;
            await _repo.UpdateAsync(originalOrder);

            _logger.LogInformation($"Order with id {originalOrder.Id} was updated");
            return Unit.Value;
        }
    }
}
