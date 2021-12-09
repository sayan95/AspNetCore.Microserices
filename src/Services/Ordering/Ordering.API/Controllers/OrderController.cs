using Microsoft.AspNetCore.Mvc;
using Ordering.BusinessLogic.Features.Orders.Commands.CheckoutOrder;
using Ordering.BusinessLogic.Features.Orders.Commands.DeleteOrder;
using Ordering.BusinessLogic.Features.Orders.Commands.UpdateOrder;
using Ordering.BusinessLogic.Features.Orders.Queries.GetOrdersList;
using Ordering.Domain.Entities;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Ordering.API.Controllers
{
    public class OrderController : BaseController
    {
        [HttpGet("{userName}", Name = "GetOrder")]
        [ProducesResponseType(typeof(IEnumerable<OrdersVm>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrdersByUserName(string userName)
        {
            var query = new GetOrdersListQuery(userName);
            var orders = await Mediator.Send(query);

            return HandleResult(orders);
        }

        [HttpPost(Name = "CheckoutOrder")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CheckoutOrder([FromBody] Order order)
        {
            var command = new CheckoutOrderCommand { Order = order };
            var orderCreated = await Mediator.Send(command);

            return HandleResult(orderCreated);
        }

        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> UpdateOrder([FromBody] Order order)
        {
            var command = new UpdateOrderCommand { Order = order };
            var orderUpdated = await Mediator.Send(command);
            return HandleResult(orderUpdated);
        }

        [HttpDelete("{id}", Name = "DeleteOrder")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var command = new DeleteOrderCommand { Id = id };
            var orderDeleted = await Mediator.Send(command);
            return HandleResult(orderDeleted);
        }
    }
}
