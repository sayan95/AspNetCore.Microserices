using Catalog.API.DataAccess;
using Catalog.API.Repositories.Class;
using Catalog.API.Repositories.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Extensions
{
    public static class CatalogExtension
    {
        public static IServiceCollection AddCatalogServices(this IServiceCollection services) {

            services.AddTransient<ICatalogContext, CatalogContext>()
                    .AddTransient<IProductRepository, ProductRepository>();
            
            return services;
        }
    }
}
