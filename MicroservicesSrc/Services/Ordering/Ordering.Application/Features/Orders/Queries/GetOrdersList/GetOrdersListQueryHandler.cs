using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrderDTO>>
    {
        private readonly IOrderRepository _repo;
        private readonly IMapper _mapper;

        public GetOrdersListQueryHandler(IOrderRepository repo, IMapper mapper)
            => (_repo, _mapper) = (repo, mapper);

        public async Task<List<OrderDTO>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
        {
            var orders = await _repo.GetOrdersByUserName(request.UserName);
            return _mapper.Map<List<OrderDTO>>(orders);
        }
    }
}
