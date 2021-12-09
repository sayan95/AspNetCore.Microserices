using MediatR;
using Ordering.BusinessLogic.Core;
using Ordering.Domain.Entities;

namespace Ordering.BusinessLogic.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommand : IRequest<Result<int>>
    {
        public Order Order { get; set; }
    }
}
