using MediatR;
using Ordering.BusinessLogic.Core;
using Ordering.Domain.Entities;

namespace Ordering.BusinessLogic.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommand : IRequest<Result<Unit>>
    {
        public Order Order { get; set; }
    }
}
