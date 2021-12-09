using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.BusinessLogic.Contracts.Persistence;
using Ordering.BusinessLogic.Core;
using Ordering.BusinessLogic.Core.Exceptions;
using Ordering.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.BusinessLogic.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, Result<List<OrdersVm>>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetOrdersListQueryHandler> _logger;

        public GetOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<GetOrdersListQueryHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<List<OrdersVm>>> Handle(GetOrdersListQuery request, 
            CancellationToken cancellationToken)
        {
            var orderList = await _orderRepository.GetOrdersByUserName(request.UserName);
            if(orderList.Count() == 0)
            {
                _logger.LogError("No orders found associated with {UserName}", request.UserName);
                return null;
            }

            var result = _mapper.Map<List<OrdersVm>>(orderList);
            return Result<List<OrdersVm>>.Success(result);
        }
    }
}
