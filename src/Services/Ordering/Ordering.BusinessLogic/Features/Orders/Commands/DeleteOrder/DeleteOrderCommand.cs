using MediatR;
using Ordering.BusinessLogic.Core;

namespace Ordering.BusinessLogic.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommand : IRequest<Result<Unit>>
    {
        public int Id { get; set; }
    }
}
