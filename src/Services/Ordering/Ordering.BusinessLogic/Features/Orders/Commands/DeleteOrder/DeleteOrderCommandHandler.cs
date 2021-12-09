using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.BusinessLogic.Contracts.Persistence;
using Ordering.BusinessLogic.Core;
using Ordering.BusinessLogic.Core.Exceptions;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.BusinessLogic.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Result<Unit>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<DeleteOrderCommandHandler> _logger;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger<DeleteOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToDelete = await _orderRepository.GetById(request.Id);
            if(orderToDelete == null)
            {
                throw new NotFoundException(nameof(Order), request.Id);
            }

            var isDeleted = await _orderRepository.DeleteAsync(orderToDelete) > 0;

            if (!isDeleted)
            {
                _logger.LogError($"Order was not dleted due to a failure in deletion process.Check the payload : {orderToDelete}");
                return Result<Unit>.Failure("Order was not dleted due to a failure in deletion process.");
            }

            _logger.LogInformation($"Order {request.Id} is successfully updated");
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
