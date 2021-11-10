using Discount.API.Repositories.Classes;
using Discount.API.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Discount.API.Extensions
{
    public static class DiscountExtensions
    {
        public static IServiceCollection AddDiscountServices(this IServiceCollection services) {
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            return services;
        }
    }
}
