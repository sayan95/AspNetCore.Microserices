﻿using Basket.API.Repositories.Classes;
using Basket.API.Repositories.Interfaces;
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

            // add services to DI container
            services.AddScoped<IBasketRepository, BasketRepository>();
            return services;
        }
    }
}
