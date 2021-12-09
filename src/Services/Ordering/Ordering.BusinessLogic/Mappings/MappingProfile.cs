using AutoMapper;
using Ordering.BusinessLogic.Features.Orders.Queries.GetOrdersList;
using Ordering.Domain.Entities;

namespace Ordering.BusinessLogic.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrdersVm>().ReverseMap();
            CreateMap<Order, Order>().ReverseMap();
        }
    }
}
