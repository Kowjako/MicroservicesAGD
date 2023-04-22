using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediatR;
        private readonly ILogger<BasketCheckoutConsumer> _logger;

        public BasketCheckoutConsumer(IMapper mapper, IMediator mediatR, ILogger<BasketCheckoutConsumer> logger)
            => (_mapper, _mediatR, _logger) = (mapper, mediatR, logger);

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var order = _mapper.Map<OrderDTO>(context.Message);
            var cmd = new CheckoutOrderCommand()
            {
                Order = order
            };

            _ = await _mediatR.Send(cmd);
            _logger.LogInformation("BasketCheckoutEvent was consumed successfully");
        }
    }
}
