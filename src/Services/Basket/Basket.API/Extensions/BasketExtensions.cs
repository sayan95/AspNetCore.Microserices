using Basket.API.GrpcServices;
using Basket.API.Repositories.Classes;
using Basket.API.Repositories.Interfaces;
using Discount.gRPC.Protos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.API.Extensions
{
    public static class BasketExtensions
    {
        public static IServiceCollection AddBasketServices(this IServiceCollection services, IConfiguration configuration) {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetSection("Redis").GetValue<string>("ConnectionString");
            });

            // adds services to DI container
            services.AddScoped<IBasketRepository, BasketRepository>();

            // adds grpc client service
            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(option =>
            {
                option.Address = new System.Uri(configuration["GrpcSettings:DiscountGrpcUrl"]);
            });
            // adds grpc service to application scope
            services.AddScoped<DiscountGrpcService>();

            return services;
        }
    }
}
