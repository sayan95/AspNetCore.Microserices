using MediatR;
using Ordering.BusinessLogic.Core;
using System.Collections.Generic;

namespace Ordering.BusinessLogic.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQuery : IRequest<Result<List<OrdersVm>>>
    {
        public string UserName { get; set; }
        public GetOrdersListQuery(string userName)
        {
            UserName = userName; 
        }

    }
}
