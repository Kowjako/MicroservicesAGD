using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
    {
        private readonly IOrderRepository _repo;
        private readonly ILogger<DeleteOrderCommandHandler> _logger;

        public DeleteOrderCommandHandler(IOrderRepository repo, ILogger<DeleteOrderCommandHandler> logger)
            => (_repo, _logger) = (repo, logger);

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _repo.GetByIdAsync(request.OrderId);
            if (order is null)
            {
                _logger.LogError($"Cant find order with id = {request.OrderId}");
                throw new NotFoundException(nameof(Order), request.OrderId);
            }

            await _repo.DeleteByIdAsync(request.OrderId);
            _logger.LogInformation($"Order with id = {request.OrderId} was deleted");
            return Unit.Value;
        }
    }
}
