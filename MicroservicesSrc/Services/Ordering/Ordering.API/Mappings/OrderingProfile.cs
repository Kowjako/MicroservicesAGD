using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;

namespace Ordering.API.Mappings
{
    public class OrderingProfile : Profile
    {
        public OrderingProfile()
        {
            CreateMap<BasketCheckoutEvent, OrderDTO>().ForMember(p => p.Id, ev => ev.Ignore());
        }
    }
}
