using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.BusinessLogic.Contracts.Infrastructure;
using Ordering.BusinessLogic.Contracts.Persistence;
using Ordering.BusinessLogic.Core;
using Ordering.BusinessLogic.Models;
using Ordering.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.BusinessLogic.Features.Orders.Commands.CheckoutOrder
{
    class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, Result<int>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;
        public CheckoutOrderCommandHandler(IOrderRepository orderRepository, 
            IMapper mapper, 
            IEmailService emailService,
            ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<Result<int>> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request.Order);
            var orderCreated = await _orderRepository.AddAsync(orderEntity);

            _logger.LogInformation($"Order {orderCreated.Id} is successfully created");

            await SendMailAsync(orderCreated);

            return Result<int>.Success(orderCreated.Id);
        }

        private async Task SendMailAsync(Order order)
        {
            var email = new Email { To = order.EmailAddress, Body = $"Order was created.", Subject = "Order was created" };
            try
            {
                await _emailService.SendEmail(email);
            }catch(Exception e)
            {
                _logger.LogError($"Order {order.Id} failed due to an error with the email service: {e.Message}");
            }
        }
    }
}
