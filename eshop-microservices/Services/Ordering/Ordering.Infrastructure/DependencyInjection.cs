using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices
            (this IServiceCollection services, IConfiguration configuration)
        {
            // Register application services here
            // Example: services.AddScoped<IOrderService, OrderService>();

            var connectionString = configuration.GetConnectionString("Database");


            return services;
        }
    }
}
