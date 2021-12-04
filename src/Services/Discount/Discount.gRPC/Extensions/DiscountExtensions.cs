using Discount.gRPC.Repositories.Classes;
using Discount.gRPC.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Discount.gRPC.Extensions
{
    public static class DiscountExtensions
    {
        public static IServiceCollection AddDiscountServices(this IServiceCollection services) {
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            return services;
        }
    }
}
