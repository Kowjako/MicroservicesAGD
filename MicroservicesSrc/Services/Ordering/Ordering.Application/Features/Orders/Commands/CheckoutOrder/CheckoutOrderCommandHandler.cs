using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _repo;
        private readonly IMapper _mapper;
        private readonly IEmailService _mailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler(IOrderRepository repo,
                                           IEmailService mailService,
                                           IMapper mapper,
                                           ILogger<CheckoutOrderCommandHandler> logger)
        {
            _repo = repo;
            _mailService = mailService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(request.Order);
            var createdOrder = await _repo.AddAsync(order);
            _logger.LogInformation($"Order created with id = {createdOrder.Id}");

            await SendMail(createdOrder.Id);
            return createdOrder.Id;
        }

        private async Task SendMail(int orderId)
        {
            var email = new Email
            {
                To = "sample@gmail.com",
                Body = $"Order {orderId} was created",
                Subject = "Order was created"
            };

            try
            {
                await _mailService.SendEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot send an email for order with an id = {orderId}", ex);
            }
        }
    }
}
