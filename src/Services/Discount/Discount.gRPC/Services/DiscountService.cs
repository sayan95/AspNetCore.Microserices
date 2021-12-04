using AutoMapper;
using Discount.gRPC.Entities;
using Discount.gRPC.Protos;
using Discount.gRPC.Repositories.Interfaces;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Discount.gRPC.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;

        public DiscountService(IDiscountRepository discountRepository, 
            ILogger<DiscountService> logger,
            IMapper mapper)
        {
            _discountRepository = discountRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _discountRepository.GetDiscount(request.ProductName);
            if(coupon == null)
            {
                _logger.LogError($"discount coupon not found with the product name={request.ProductName}");
                throw new RpcException(new Status(StatusCode.NotFound, $"Disccount with product name={request.ProductName} was not found."));
            }

            _logger.LogInformation("Discount is retrieved for ProductName: {0} Amount: {1}", coupon.ProductName, coupon.Amount);
            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            var isCreated = await _discountRepository.CreateDiscount(coupon);

            if (!isCreated)
            {
                _logger.LogError("Internal server error, unable to create new discount coupon.");
                throw new RpcException(new Status(StatusCode.Internal, $"Discount coupon can not be created!"));
            }

            _logger.LogInformation("Discount is created successfully. ProductName: {0}", coupon.ProductName);

            return request.Coupon;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            var isUpdated = await _discountRepository.UpdateDiscount(coupon);

            if (!isUpdated)
            {
                _logger.LogError("Internal server error, unable to update discount coupon for {0}", coupon.ProductName);
                throw new RpcException(new Status(StatusCode.Internal, $"Discount coupon can not be updated!"));
            }

            _logger.LogInformation("Discount coupon is updated successfully. ProductName: {0}", coupon.ProductName);

            return request.Coupon;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {

            var isDeleted = await _discountRepository.DeleteDiscount(request.ProductName);

            if (!isDeleted)
            {
                _logger.LogError("Internal server error, unable to delete discount coupon for {0}", request.ProductName);
                throw new RpcException(new Status(StatusCode.Internal, $"Discount coupon can not be deleted!"));
            }

            _logger.LogInformation("Discount coupon is updated successfully. ProductName: {0}", request.ProductName);

            return new DeleteDiscountResponse
            {
                Success = isDeleted
            };
        }
    }
}
