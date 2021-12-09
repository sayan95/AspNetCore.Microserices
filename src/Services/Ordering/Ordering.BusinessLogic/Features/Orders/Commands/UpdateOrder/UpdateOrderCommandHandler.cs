using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.BusinessLogic.Contracts.Persistence;
using Ordering.BusinessLogic.Core;
using Ordering.BusinessLogic.Core.Exceptions;
using Ordering.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.BusinessLogic.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Result<Unit>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository, 
            IMapper mapper, 
            ILogger<UpdateOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await _orderRepository.GetById(request.Order.Id);

            if(orderToUpdate == null)
            {
                _logger.LogError("No order with {id} was found", request.Order.Id);
                throw new NotFoundException(nameof(Order), request.Order.Id);
            }

            _mapper.Map(request.Order, orderToUpdate, typeof(Order), typeof(Order));

            var isUpdated = await _orderRepository.UpdateAsync(orderToUpdate) > 0;

            if(!isUpdated)
            {
                _logger.LogError($"Order was not updated due to a failure in updation process.Check the payload : {orderToUpdate}");
                return Result<Unit>.Failure("Order was not updated due to a failure in updation process.");
            }  

            _logger.LogInformation($"Order {orderToUpdate.Id} is successfully updated");
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
