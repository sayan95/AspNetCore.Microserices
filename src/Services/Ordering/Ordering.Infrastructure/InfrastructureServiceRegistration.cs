using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.BusinessLogic.Contracts.Infrastructure;
using Ordering.BusinessLogic.Contracts.Persistence;
using Ordering.BusinessLogic.Models;
using Ordering.Infrastructure.Mail;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderContext>(option =>
                option.UseSqlServer(configuration.GetConnectionString("OrderConnectionString"))
            );
                
            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddTransient<IEmailService, EmailService>();
            services.Configure<EmailSettings>(c => configuration.GetSection("SendGridSettings"));

            return services;
        }
    }
}
